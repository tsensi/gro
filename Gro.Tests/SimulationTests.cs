using Gro.ECS;
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
}
