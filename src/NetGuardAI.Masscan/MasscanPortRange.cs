namespace NetGuardAI.Masscan;

public class MasscanPortRange(int fromPort, int? toPort = null)
{
    public int FromPort { get; } = fromPort;
    public int? ToPort { get; } = toPort;

    public override string ToString()
        => ToPort.HasValue ? $"{FromPort}-{ToPort.Value}" : $"{FromPort}";
}