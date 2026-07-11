using Gro.ECS;
using Gro.Infection;
using Gro.Services;
using Gro.Simulation;
using Xunit;

namespace Gro.Tests;

public class SimulationTests
{
    [Fact]
    public void Tick_AdvancesElapsedTime()
    {
        var sim = new SimLoop();
        sim.Tick(1.0);
        Assert.Equal(1.0, sim.State.ElapsedDays);
        sim.Tick(0.5);
        Assert.Equal(1.5, sim.State.ElapsedDays);
    }

    [Fact]
    public void Tick_SetsDeltaDays()
    {
        var sim = new SimLoop();
        sim.Tick(2.5);
        Assert.Equal(2.5, sim.State.DeltaDays);
    }

    [Fact]
    public void Tick_WhenPaused_DoesNotAdvance()
    {
        var sim = new SimLoop();
        sim.Paused = true;
        sim.Tick(1.0);
        Assert.Equal(0.0, sim.State.ElapsedDays);
    }

    [Fact]
    public void Tick_AppliesTimeScale()
    {
        var sim = new SimLoop();
        sim.TimeScale = 2.0;
        sim.Tick(1.0);
        Assert.Equal(2.0, sim.State.ElapsedDays);
        Assert.Equal(2.0, sim.State.DeltaDays);
    }

    [Fact]
    public void Tick_CallsAllSystems()
    {
        var sim = new SimLoop();
        var counter = new CounterSystem();
        sim.AddSystem(counter);
        sim.Tick(1.0);
        sim.Tick(1.0);
        Assert.Equal(2, counter.TickCount);
    }

    [Fact]
    public void Tick_SystemsReceiveCorrectState()
    {
        var sim = new SimLoop();
        var recorder = new RecorderSystem();
        sim.AddSystem(recorder);
        sim.Tick(3.0);
        Assert.Equal(3.0, recorder.LastDelta);
        Assert.Equal(3.0, recorder.LastElapsed);
        Assert.Same(sim.World, recorder.LastWorld);
    }

    [Fact]
    public void Tick_SystemsExecuteInOrder()
    {
        var sim = new SimLoop();
        var log = new List<string>();
        sim.AddSystem(new LogSystem(log, "A"));
        sim.AddSystem(new LogSystem(log, "B"));
        sim.AddSystem(new LogSystem(log, "C"));
        sim.Tick(1.0);
        Assert.Equal(new[] { "A", "B", "C" }, log);
    }

    [Fact]
    public void World_IsAccessibleFromSimulation()
    {
        var sim = new SimLoop();
        var entity = sim.World.Spawn();
        Assert.True(sim.World.IsAlive(entity));
    }

    [Fact]
    public void Constructor_AcceptsExistingWorld()
    {
        var world = new World();
        var entity = world.Spawn();
        var sim = new SimLoop(world);
        Assert.Same(world, sim.World);
        Assert.True(sim.World.IsAlive(entity));
    }

    [Fact]
    public void Tick_SystemCanModifyWorld()
    {
        var sim = new SimLoop();
        sim.AddSystem(new SpawnerSystem());
        sim.Tick(1.0);
        var entities = sim.World.Query<ZoneLink>().ToList();
        Assert.Single(entities);
        Assert.Equal("Spawned", sim.World.Get<ZoneLink>(entities[0])!.ZoneName);
    }

    private sealed class CounterSystem : ISystem
    {
        public int TickCount { get; private set; }
        public void Tick(SimState state) => TickCount++;
    }

    private sealed class RecorderSystem : ISystem
    {
        public double LastDelta { get; private set; }
        public double LastElapsed { get; private set; }
        public World? LastWorld { get; private set; }

        public void Tick(SimState state)
        {
            LastDelta = state.DeltaDays;
            LastElapsed = state.ElapsedDays;
            LastWorld = state.World;
        }
    }

    private sealed class LogSystem : ISystem
    {
        private readonly List<string> _log;
        private readonly string _name;

        public LogSystem(List<string> log, string name)
        {
            _log = log;
            _name = name;
        }

        public void Tick(SimState state) => _log.Add(_name);
    }

    private sealed class SpawnerSystem : ISystem
    {
        public void Tick(SimState state)
        {
            var entity = state.World.Spawn();
            state.World.Set(entity, new ZoneLink { ZoneName = "Spawned" });
        }
    }

    [Fact]
    public void XenoGrowth_IncreasesOverTime()
    {
        var sim = new SimLoop();
        sim.AddSystem(new XenoGrowthSystem());
        var entity = sim.World.SpawnInZone("TestZone");
        sim.World.Set(entity, new InfectionComponent { Biomass = 10.0 });

        sim.Tick(1.0);

        var infection = sim.World.Get<InfectionComponent>(entity)!;
        Assert.True(infection.Biomass > 10.0);
    }

    [Fact]
    public void XenoGrowth_DoublesIn100Days()
    {
        var sim = new SimLoop();
        sim.AddSystem(new XenoGrowthSystem());
        var entity = sim.World.SpawnInZone("TestZone");
        sim.World.Set(entity, new InfectionComponent { Biomass = 10.0 });

        double hundredDays = 100.0;
        int steps = 1000;
        double stepSize = hundredDays / steps;
        for (int i = 0; i < steps; i++)
            sim.Tick(stepSize);

        var infection = sim.World.Get<InfectionComponent>(entity)!;
        Assert.InRange(infection.Biomass, 19.5, 20.5);
    }

    [Fact]
    public void XenoGrowth_MultipleEntities()
    {
        var sim = new SimLoop();
        sim.AddSystem(new XenoGrowthSystem());
        var e1 = sim.World.SpawnInZone("Zone1");
        sim.World.Set(e1, new InfectionComponent { Biomass = 10.0 });
        var e2 = sim.World.SpawnInZone("Zone2");
        sim.World.Set(e2, new InfectionComponent { Biomass = 100.0 });

        sim.Tick(365.25);

        var b1 = sim.World.Get<InfectionComponent>(e1)!.Biomass;
        var b2 = sim.World.Get<InfectionComponent>(e2)!.Biomass;
        Assert.True(b1 > 10.0);
        Assert.True(b2 > 100.0);
        Assert.InRange(b2 / b1, 9.5, 10.5);
    }

    [Fact]
    public void BiomassHarvest_AddsPerInfectedZonePerDay()
    {
        ServiceLocator.Clear();
        var resources = new ResourceService();
        ServiceLocator.Register(resources);

        var sim = new SimLoop();
        sim.AddSystem(new BiomassHarvestSystem());
        sim.World.SpawnInZone("Zone1");
        sim.World.Set(sim.World.EntitiesInZone("Zone1").First(), new InfectionComponent());
        sim.World.SpawnInZone("Zone2");
        sim.World.Set(sim.World.EntitiesInZone("Zone2").First(), new InfectionComponent());

        sim.Tick(1.0);

        Assert.Equal(2.0, resources.Biomass, precision: 10);
        ServiceLocator.Clear();
    }

    [Fact]
    public void BiomassHarvest_ScalesWithDeltaDays()
    {
        ServiceLocator.Clear();
        var resources = new ResourceService();
        ServiceLocator.Register(resources);

        var sim = new SimLoop();
        sim.AddSystem(new BiomassHarvestSystem());
        sim.World.SpawnInZone("Zone1");
        sim.World.Set(sim.World.EntitiesInZone("Zone1").First(), new InfectionComponent());

        sim.Tick(0.5);

        Assert.Equal(0.5, resources.Biomass, precision: 10);
        ServiceLocator.Clear();
    }

    [Fact]
    public void BiomassHarvest_NoInfectedZones_NoBiomass()
    {
        ServiceLocator.Clear();
        var resources = new ResourceService();
        ServiceLocator.Register(resources);

        var sim = new SimLoop();
        sim.AddSystem(new BiomassHarvestSystem());

        sim.Tick(1.0);

        Assert.Equal(0.0, resources.Biomass);
        ServiceLocator.Clear();
    }

    [Fact]
    public void BiomassHarvest_Accumulates()
    {
        ServiceLocator.Clear();
        var resources = new ResourceService();
        ServiceLocator.Register(resources);

        var sim = new SimLoop();
        sim.AddSystem(new BiomassHarvestSystem());
        var e1 = sim.World.SpawnInZone("Zone1");
        sim.World.Set(e1, new InfectionComponent());

        sim.Tick(1.0);
        sim.Tick(1.0);
        sim.Tick(1.0);

        Assert.Equal(3.0, resources.Biomass, precision: 10);
        ServiceLocator.Clear();
    }
}
