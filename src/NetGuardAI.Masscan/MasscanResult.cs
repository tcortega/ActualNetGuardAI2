using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace NetGuardAI.Masscan;

public class MasscanResult
{
    public bool Success { get; init; }

    public IReadOnlyList<MasscanServer> FoundHosts { get; init; } = new List<MasscanServer>();
}