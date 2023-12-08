using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetGuardAI.Nmap;

public class NmapWrapper
{
    private readonly string _executablePath;

    public NmapWrapper()
    {
        _executablePath = "nmap";
    }
    
    private NmapWrapper(string executablePath)
    {
        _executablePath = executablePath;
    }

    public static async Task<NmapWrapper> CreateAsync(string executablePath = "nmap")
    {
        await ValidateNmapInstallationAsync(executablePath);
        return new NmapWrapper(executablePath);
    }

    public Task<NmapResult> ScanAsync(string ipAddress, int port)
        => ScanAsync(IPAddress.Parse(ipAddress), port);

    public async Task<NmapResult> ScanAsync(IPAddress ipAddress, int port)
    {
        var reportId = Guid.NewGuid().ToString();
        var reportPath = Path.Combine(Path.GetTempPath(), reportId + ".xml");
        var argumentBuilder = new StringBuilder("-Pn -sS -T4 -A -PE -PP -PY -g 53 --script=http-headers,http-title,banner");
        // var argumentBuilder = new StringBuilder("-T4 -A --script banner");
        var isIpv6 = ipAddress.AddressFamily is AddressFamily.InterNetworkV6;
        if (isIpv6)
        {
            argumentBuilder.Append(" -6");
        }
        
        argumentBuilder.Append($" -p {port}");
        argumentBuilder.Append($" -oX {reportPath}");
        argumentBuilder.Append($" {ipAddress}");
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _executablePath,
                Arguments = argumentBuilder.ToString(),
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = true,
                // RedirectStandardError = true,
                // RedirectStandardOutput = true
            }
        };

        process.Start();
        await process.WaitForExitAsync();
        
        return new NmapResult
        {
            Success = process.ExitCode == 0,
            ReportPath = reportPath,
        };
    }

    private static async Task ValidateNmapInstallationAsync(string executablePath)
    {
        if (executablePath != "nmap" && !File.Exists(executablePath))
        {
            throw new FileNotFoundException("Could not find nmap executable", executablePath);
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = "--version",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (!output.Contains("Nmap version"))
        {
            throw new Exception("Could not find nmap executable");
        }
    }
}