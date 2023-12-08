namespace NetGuardAI.Core.Persistence.Entities;

public class PortRange : BaseEntity
{
    public required int FromPort { get; set; }
    public int? ToPort { get; set; }
}