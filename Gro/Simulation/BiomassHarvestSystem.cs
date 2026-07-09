using Gro.Infection;
using Gro.Services;

namespace Gro.Simulation;

public sealed class BiomassHarvestSystem : ISystem
{
    public void Tick(SimState state)
    {
        int infectedCount = 0;
        foreach (var _ in state.World.Query<InfectionComponent>())
            infectedCount++;

        if (infectedCount > 0)
        {
            var resources = ServiceLocator.Get<ResourceService>();
            resources.Add(infectedCount * state.DeltaDays);
        }
    }
}
