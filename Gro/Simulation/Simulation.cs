using Gro.ECS;

namespace Gro.Simulation;

public sealed class SimLoop
{
    private readonly List<ISystem> _systems = new();

    public SimState State { get; }
    public World World => State.World;
    public bool Paused { get; set; }
    public double TimeScale { get; set; } = 1.0;

    public SimLoop()
    {
        State = new SimState(new World());
    }

    public SimLoop(World world)
    {
        State = new SimState(world);
    }

    public void AddSystem(ISystem system)
    {
        _systems.Add(system);
    }

    public void Tick(double deltaDays)
    {
        if (Paused)
            return;

        double scaled = deltaDays * TimeScale;
        State.DeltaDays = scaled;
        State.ElapsedDays += scaled;

        foreach (var system in _systems)
            system.Tick(State);
    }
}
