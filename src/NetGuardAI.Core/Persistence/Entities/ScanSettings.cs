using NodaTime;

namespace NetGuardAI.Core.Persistence.Entities;

public class ScanSettings : BaseEntity
{
    public required int MasscanRate { get; set; }
    public required int NmapConcurrencyLimit { get; set; }
    public required Duration IpCooldown { get; set; }
}