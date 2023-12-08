using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.RegularExpressions;

namespace NetGuardAI.Masscan;

public partial class MasscanServer
{
    public required IPAddress Ip { get; set; }
    public required int Port { get; set; }
    
    public static bool TryParse(string input, [NotNullWhen(true)] out MasscanServer? result)
    {
        try
        {
            result = Parse(input);
            return true;
        }
        catch (Exception)
        {
            result = null;
            return false;
        }
    }
    
    public static MasscanServer Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        if (!input.StartsWith("Discovered open port"))
        {
            throw new FormatException("Input is not in the correct format.");
        }
        
        var match = MasscanOutputRegex().Match(input);
        if (!match.Success)
        {
            throw new FormatException("Input is not in the correct format.");
        }
        
        var port = int.Parse(match.Groups[1].Value);
        var ip = IPAddress.Parse(match.Groups[2].Value);
        
        return new MasscanServer
        {
            Ip = ip,
            Port = port
        };
    }

    [GeneratedRegex("""Discovered open port (\d+)/tcp on (\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b|\b(?:[A-Fa-f0-9]{1,4}:){7}[A-Fa-f0-9]{1,4}\b)""")]
    private static partial Regex MasscanOutputRegex();
}