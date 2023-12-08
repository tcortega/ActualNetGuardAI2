using System.Text;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using NetGuardAI.Core.Hangfire;
using NetGuardAI.Core.Persistence;
using NetGuardAI.Core.Persistence.Entities;
using NetGuardAI.Masscan;
using NetGuardAI.Nmap;
using NodaTime;

namespace NetGuardAI.Core.Features;

[DisableConcurrentExecution(30 * 60)]
[RecurringJob(RecurringJobId = "NetworkScanningJob", Cron = "*/10 * * * *")]
public class NetworkScanningJob(IClock clock, AppDbContext context, IDbContextFactory<AppDbContext> contextFactory,
    NetworkScanner scanner,
    ScanResultProcessor resultProcessor) : IRecurringJob
{
    private readonly HttpClient _httpClient = new();
    private List<UserWebhook> _webhooks = new();

    public async Task ExecuteAsync()
    {
        _webhooks = await context.UserWebhooks.ToListAsync();

        var targets = await context.ScanTargets.ToListAsync();
        if (targets.Count == 0) return;

        var portRanges = await context.PortRanges.ToListAsync();
        if (portRanges.Count == 0) return;

        await scanner.ScanNetworkAsync(OnScanResult, targets, portRanges);
    }

    private async Task OnScanResult(MasscanServer server, NmapResult nmapResult, string? httpContent)
    {
        var rawResult = await File.ReadAllTextAsync(nmapResult.ReportPath!);

        var processedResult = await resultProcessor.ProcessScanResultAsync(rawResult, httpContent);

        foreach (var webhook in _webhooks)
        {
            var content = new StringContent(processedResult.Output, Encoding.UTF8, "application/json");
            _ = _httpClient.PostAsync(webhook.Url, content);
        }

        var scanResult = new ScanResult
        {
            IpAddress = server.Ip,
            Port = server.Port,
            ProcessedInfo = processedResult.Output,
            RawInfo = processedResult.RawInput,
            ScanTime = clock.GetCurrentInstant()
        };
        
        var dbContext = contextFactory.CreateDbContext();
        dbContext.ScanResults.Add(scanResult);
        await dbContext.SaveChangesAsync();
    }
}