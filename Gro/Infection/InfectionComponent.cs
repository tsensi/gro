namespace Gro.Infection;

public sealed class InfectionComponent
{
    public double Biomass { get; set; } = 10.0;
    public int GrowthLevel { get; set; } = 1;

    public double GrowthMultiplier => 1.0 + (GrowthLevel - 1) * 0.5;

    public static int UpgradeCost(int currentLevel) => 10 * currentLevel;
}
