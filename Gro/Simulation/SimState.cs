using Gro.ECS;

namespace Gro.Simulation;

public sealed class SimState
{
    public World World { get; }
    public double DeltaDays { get; internal set; }
    public double ElapsedDays { get; internal set; }

    internal SimState(World world)
    {
        World = world;
    }
}
