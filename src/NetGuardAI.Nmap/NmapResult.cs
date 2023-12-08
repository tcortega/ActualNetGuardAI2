using System.Diagnostics.CodeAnalysis;

namespace NetGuardAI.Nmap;

public class NmapResult
{
    [MemberNotNullWhen(true, nameof(ReportPath))]
    public bool Success { get; init; }
    public string? ReportPath { get; init; }
}