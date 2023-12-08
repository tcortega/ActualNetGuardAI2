namespace NetGuardAI.Core.Persistence.Entities;

public class ScanTarget : BaseEntity
{
    public required string IpRange { get; set; }
}