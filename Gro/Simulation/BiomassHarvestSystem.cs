using Gro.Infection;
using Gro.Services;

namespace Gro.Simulation;

public sealed class BiomassHarvestSystem : ISystem
{
    private readonly ResourceService _resources;

    public BiomassHarvestSystem(ResourceService resources)
    {
        _resources = resources;
    }

    public void Tick(SimState state)
    {
        int infectedCount = 0;
        foreach (var _ in state.World.Query<InfectionComponent>())
            infectedCount++;

        if (infectedCount > 0)
        {
            _resources.Add(infectedCount * state.DeltaDays);
        }
    }
}
