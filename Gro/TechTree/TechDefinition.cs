namespace Gro.TechTree;

public sealed class TechDefinition
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required int Tier { get; init; }
    public string? Prerequisite { get; init; }
    public double ResearchDays { get; init; } = 30.0;
    public double DiffusionRatePerDay { get; init; } = 0.02;
}
