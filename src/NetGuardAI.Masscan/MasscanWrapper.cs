using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NetGuardAI.Masscan;

public class MasscanWrapper
{
    private readonly string _executablePath = GetDefaultExecutablePath();

    public Task<MasscanResult> ScanAsync(MasscanOutputDelegate outputDelegate, string target, int fromPort = 1,
        int toPort = 65535,
        int rate = 1000)
        => ScanAsync(outputDelegate, new[] { target }, fromPort, toPort, rate);

    public Task<MasscanResult> ScanAsync(MasscanOutputDelegate outputDelegate, IEnumerable<string> targets,
        int fromPort = 1, int toPort = 65535,
        int rate = 1000)
        => ScanAsync(outputDelegate, targets.Select(IPNetwork.Parse), fromPort, toPort, rate);

    public Task<MasscanResult> ScanAsync(MasscanOutputDelegate outputDelegate, IEnumerable<IPNetwork> targets,
        int fromPort = 1, int toPort = 65535,
        int rate = 1000)
        => ScanAsync(outputDelegate, targets, new MasscanPortRange(fromPort, toPort), rate);

    public Task<MasscanResult> ScanAsync(MasscanOutputDelegate outputDelegate, IEnumerable<IPNetwork> targets,
        MasscanPortRange portRange,
        int rate = 1000)
        => ScanAsync(outputDelegate, targets, new[] { portRange }, rate);

    public async Task<MasscanResult> ScanAsync(MasscanOutputDelegate outputDelegate, IEnumerable<IPNetwork> targets,
        IEnumerable<MasscanPortRange> portRange,
        int rate = 1000)
    {
        var targetString = string.Join(" ", targets.Select(x => x.ToString()));
        var portRangeString = string.Join(",", portRange.Select(x => x.ToString()));
        var arguments = $"{targetString} -p{portRangeString} --rate={rate}";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _executablePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        var foundHosts = new List<MasscanServer>();
        process.OutputDataReceived += (_, args) =>
        {
            if (args.Data is null) return;
            if (!MasscanServer.TryParse(args.Data, out var server)) return;

            foundHosts.Add(server);
            outputDelegate(server);
        };

        process.Start();
        process.BeginOutputReadLine();
        await process.WaitForExitAsync();
        
        return new MasscanResult
        {
            Success = process.ExitCode == 0,
            FoundHosts = foundHosts
        };
    }

    private static string GetDefaultExecutablePath()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new NotSupportedException("This OS platform is not supported.");
        }

        var fileSuffix = Environment.Is64BitOperatingSystem ? "64" : "32";
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        return Path.Combine(basePath, $"libs/masscan{fileSuffix}.exe");
    }
}