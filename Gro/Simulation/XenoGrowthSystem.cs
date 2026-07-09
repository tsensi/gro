using Gro.Infection;

namespace Gro.Simulation;

public sealed class XenoGrowthSystem : ISystem
{
    // ln(2) / 3652.5 days ≈ doubling time of 10 years
    public const double GrowthRate = 0.00018971199848858813;

    public void Tick(SimState state)
    {
        foreach (var entity in state.World.Query<InfectionComponent>())
        {
            var infection = state.World.Get<InfectionComponent>(entity)!;
            infection.Biomass += infection.Biomass * GrowthRate * state.DeltaDays;
        }
    }
}
