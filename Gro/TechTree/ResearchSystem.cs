using Gro.Simulation;

namespace Gro.TechTree;

public sealed class ResearchSystem : ISystem
{
    private readonly TechRegistry _registry;

    public ResearchSystem(TechRegistry registry)
    {
        _registry = registry;
    }

    public void Tick(SimState state)
    {
        foreach (var entity in state.World.Query<ResearchComponent>())
        {
            var research = state.World.Get<ResearchComponent>(entity)!;
            foreach (var techId in research.ActiveResearch)
            {
                if (research.IsFullyEstablished(techId))
                    continue;

                var tech = _registry.Get(techId);
                if (tech == null)
                    continue;

                double progressPerDay = 1.0 / tech.ResearchDays;
                research.AddProgress(techId, progressPerDay * state.DeltaDays);
            }
        }
    }
}
