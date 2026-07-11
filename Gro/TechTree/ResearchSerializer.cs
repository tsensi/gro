using System.Text.Json;
using Gro.ECS;

namespace Gro.TechTree;

public static class ResearchSerializer
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    public static List<ResearchSaveData> Extract(World world)
    {
        var results = new List<ResearchSaveData>();
        foreach (var entity in world.Query<ResearchComponent>())
        {
            var zoneLink = world.Get<ZoneLink>(entity);
            if (zoneLink == null) continue;

            var research = world.Get<ResearchComponent>(entity)!;
            if (research.Progress.Count == 0 && research.ActiveResearch.Count == 0)
                continue;

            results.Add(new ResearchSaveData
            {
                ZoneName = zoneLink.ZoneName,
                Progress = new Dictionary<string, double>(research.Progress),
                ActiveResearch = new List<string>(research.ActiveResearch),
            });
        }
        return results;
    }

    public static void Apply(World world, List<ResearchSaveData> saves)
    {
        foreach (var save in saves)
        {
            var entity = FindEntityForZone(world, save.ZoneName);
            if (entity == null) continue;

            var research = world.Get<ResearchComponent>(entity.Value) ?? new ResearchComponent();

            foreach (var techId in save.ActiveResearch)
                research.StartResearch(techId);

            foreach (var (techId, progress) in save.Progress)
            {
                double current = research.GetProgress(techId);
                if (progress > current)
                    research.AddProgress(techId, progress - current);
            }

            world.Set(entity.Value, research);
        }
    }

    public static string ToJson(List<ResearchSaveData> saves)
    {
        return JsonSerializer.Serialize(saves, JsonOptions);
    }

    public static List<ResearchSaveData> FromJson(string json)
    {
        return JsonSerializer.Deserialize<List<ResearchSaveData>>(json, JsonOptions)
               ?? throw new InvalidOperationException("Failed to parse research save data JSON");
    }

    public static void SaveToFile(World world, string path)
    {
        var saves = Extract(world);
        var json = ToJson(saves);
        File.WriteAllText(path, json);
    }

    public static void LoadFromFile(World world, string path)
    {
        if (!File.Exists(path)) return;
        var json = File.ReadAllText(path);
        var saves = FromJson(json);
        Apply(world, saves);
    }

    private static Entity? FindEntityForZone(World world, string zoneName)
    {
        foreach (var entity in world.Query<ZoneLink>())
        {
            var link = world.Get<ZoneLink>(entity);
            if (link?.ZoneName == zoneName)
                return entity;
        }
        return null;
    }
}
