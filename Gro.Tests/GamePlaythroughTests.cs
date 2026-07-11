using Gro.ECS;
using Gro.Infection;
using Gro.Services;
using Gro.Simulation;
using Xunit;

namespace Gro.Tests;

public class GamePlaythroughTests
{
    [Fact]
    public void InfectAlgeria_AfterTicks_BiomassAndTimeAdvance()
    {
        ServiceLocator.Clear();
        var resources = new ResourceService();
        ServiceLocator.Register(resources);

        var sim = new SimLoop();
        sim.AddSystem(new XenoGrowthSystem());
        sim.AddSystem(new BiomassHarvestSystem());

        var entity = sim.World.SpawnInZone("Algeria");
        sim.World.Set(entity, new InfectionComponent { Biomass = 10.0 });

        for (int i = 0; i < 100; i++)
            sim.Tick(1.0);

        Assert.True(sim.State.ElapsedDays > 0, "Simulation time should have advanced");
        Assert.True(resources.Biomass > 0, "Biomass should be positive after ticking with an infected zone");

        ServiceLocator.Clear();
    }
}
