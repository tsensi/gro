using Gro.Infection;

namespace Gro.Simulation;

public sealed class XenoGrowthSystem : ISystem
{
    // ln(2) / 100 days ≈ doubling time of 100 game-days
    public const double GrowthRate = 0.006931471805599453;

    public void Tick(SimState state)
    {
        foreach (var entity in state.World.Query<InfectionComponent>())
        {
            var infection = state.World.Get<InfectionComponent>(entity)!;
            infection.Biomass += infection.Biomass * GrowthRate * infection.GrowthMultiplier * state.DeltaDays;
        }
    }
}
