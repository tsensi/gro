namespace Gro.TechTree;

public sealed class ResearchSaveData
{
    public required string ZoneName { get; init; }
    public required Dictionary<string, double> Progress { get; init; }
    public required List<string> ActiveResearch { get; init; }
}
