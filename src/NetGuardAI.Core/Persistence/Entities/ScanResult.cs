using System.Net;
using NodaTime;

namespace NetGuardAI.Core.Persistence.Entities;

public class ScanResult : BaseEntity
{
    public required IPAddress IpAddress { get; set; }
    public required int Port { get; set; }
    public required string RawInfo { get; set; }
    public required string ProcessedInfo { get; set; }
    public required Instant ScanTime { get; set; }
}