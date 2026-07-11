using Gro.EarthModel;
using Gro.ECS;
using Gro.Infection;
using Gro.Simulation;

namespace Gro.TechTree;

public sealed class TechDiffusionSystem : ISystem
{
    private readonly TechRegistry _registry;
    private readonly AdjacencyMap _adjacency;

    public TechDiffusionSystem(TechRegistry registry, AdjacencyMap adjacency)
    {
        _registry = registry;
        _adjacency = adjacency;
    }

    public void Tick(SimState state)
    {
        var zoneTechProgress = new Dictionary<string, Dictionary<string, double>>();

        foreach (var entity in state.World.Query<ResearchComponent>())
        {
            var zoneLink = state.World.Get<ZoneLink>(entity);
            if (zoneLink == null) continue;

            var research = state.World.Get<ResearchComponent>(entity)!;
            foreach (var (techId, progress) in research.Progress)
            {
                if (progress <= 0) continue;
                if (!zoneTechProgress.TryGetValue(zoneLink.ZoneName, out var techMap))
                {
                    techMap = new Dictionary<string, double>();
                    zoneTechProgress[zoneLink.ZoneName] = techMap;
                }
                techMap[techId] = progress;
            }
        }

        foreach (var entity in state.World.Query<InfectionComponent>())
        {
            var zoneLink = state.World.Get<ZoneLink>(entity);
            if (zoneLink == null) continue;

            var research = state.World.Get<ResearchComponent>(entity);
            if (research == null) continue;

            var neighbors = _adjacency.GetNeighbors(zoneLink.ZoneName);
            foreach (var neighborName in neighbors)
            {
                if (!zoneTechProgress.TryGetValue(neighborName, out var neighborTechs))
                    continue;

                foreach (var (techId, neighborProgress) in neighborTechs)
                {
                    if (!neighborProgress.Equals(1.0) && neighborProgress < 1.0)
                        continue;

                    var tech = _registry.Get(techId);
                    if (tech == null) continue;

                    if (research.IsFullyEstablished(techId))
                        continue;

                    double diffusionAmount = tech.DiffusionRatePerDay * state.DeltaDays;
                    research.AddProgress(techId, diffusionAmount);
                }
            }
        }
    }
}
