using System.Net;
using NetGuardAI.Core.Persistence.Entities;
using NetGuardAI.Masscan;

namespace NetGuardAI.Core.Misc;

public static class Utils
{
    public static MasscanPortRange ToMasscanPortRange(this PortRange portRange)
        => new(portRange.FromPort, portRange.ToPort);
    
    public static IPNetwork ToIpNetwork(this ScanTarget scanTarget)
        => IPNetwork.Parse(scanTarget.IpRange);
}