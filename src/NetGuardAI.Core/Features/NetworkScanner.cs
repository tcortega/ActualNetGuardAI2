using System.Net;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using NetGuardAI.Core.Misc;
using NetGuardAI.Core.Persistence;
using NetGuardAI.Core.Persistence.Entities;
using NetGuardAI.Masscan;
using NetGuardAI.Nmap;
using NodaTime;

namespace NetGuardAI.Core.Features;

public delegate Task ScanNetworkOutputDelegate(MasscanServer server, NmapResult nmapResult, string? httpContent);

[RegisterScoped]
public class NetworkScanner(AppDbContext context, IClock clock, MasscanWrapper masscan, NmapWrapper nmap)
{
    private readonly HttpClient _httpClient = new();
    
    public async Task ScanNetworkAsync(ScanNetworkOutputDelegate outputDelegate, IEnumerable<ScanTarget> ipRange, IEnumerable<PortRange> portRanges)
    {
        var targets = ipRange.Select(Utils.ToIpNetwork);
        var masscanPortRanges = portRanges.Select(Utils.ToMasscanPortRange);
        
        var settings = await context.ScanSettings.FirstAsync();

        var toleranceDate = clock.GetCurrentInstant().Minus(settings.IpCooldown);
        var lastScannedServers = await context.ScanResults.Where(x => x.ScanTime > toleranceDate)
            .ToListAsync();

        var processedServers = 0;
        var semaphore = new SemaphoreSlim(settings.NmapConcurrencyLimit, settings.NmapConcurrencyLimit);
        var masscanResult = await masscan.ScanAsync(OnMasscanResult, targets, masscanPortRanges, settings.MasscanRate).ConfigureAwait(false);
        
        // Not sure this is the best way to do it.
        while (processedServers != masscanResult.FoundHosts.Count)
        {
            await Task.Delay(200).ConfigureAwait(false);
        }

        return;

        async Task OnMasscanResult(MasscanServer server)
        {
            await semaphore.WaitAsync();

            if (!ShouldScanServer(server, lastScannedServers)) return;
            
            try
            {
                var result = await ScanIpAddress(server.Ip, server.Port);
                _ = outputDelegate(server, result.NmapResult, result.HttpContent);
            }
            catch (Exception e)
            {
                Console.WriteLine("TODO: Something went wrong nmap scanning the given ip");
                Console.WriteLine(e);
            }
            finally
            {
                Interlocked.Increment(ref processedServers);
                semaphore.Release();
            }
        }
    }

    public async Task<IpScanResult> ScanIpAddress(IPAddress ip, int port)
    {
        var nmapResult = await nmap.ScanAsync(ip, port);
        if (!nmapResult.Success)
        {
            throw new NotImplementedException("TODO: Handle nmap error");
        }
        
        var httpContent = await PerformHttpRequest(ip, port, nmapResult);
        return new IpScanResult(nmapResult, httpContent);
    }
    
    private async ValueTask<string?> PerformHttpRequest(IPAddress ip, int port, NmapResult nmapResult)
    {
        var rawNmapResult = await File.ReadAllTextAsync(nmapResult.ReportPath!);
        if (!rawNmapResult.Contains("HTTP")) return null;

        var urlPrefix = port == 443 ? "https://" : "http://";
        var url = $"{urlPrefix}{ip}:{port}";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddDefaultHeaders(request.Headers);

        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }

    private static void AddDefaultHeaders(HttpHeaders requestHeaders)
    {
        requestHeaders.TryAddWithoutValidation("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0");

        requestHeaders.TryAddWithoutValidation("Accept",
            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        
    }

    private static bool ShouldScanServer(MasscanServer server, IEnumerable<ScanResult> previousResults)
        => !previousResults.Any(x => x.IpAddress.Equals(server.Ip) && x.Port == server.Port);
}

public record IpScanResult(NmapResult NmapResult, string? HttpContent);