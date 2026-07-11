using System.Text.Json;

namespace Gro.TechTree;

public sealed class TechRegistry
{
    private readonly Dictionary<string, TechDefinition> _techs = new();

    public IReadOnlyCollection<TechDefinition> All => _techs.Values;

    public TechDefinition? Get(string id)
    {
        return _techs.TryGetValue(id, out var tech) ? tech : null;
    }

    public void Register(TechDefinition tech)
    {
        _techs[tech.Id] = tech;
    }

    public static TechRegistry LoadFromJson(string json)
    {
        var registry = new TechRegistry();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var techs = JsonSerializer.Deserialize<TechDefinition[]>(json, options)
                    ?? throw new InvalidOperationException("Failed to parse tech definitions JSON");
        foreach (var tech in techs)
            registry.Register(tech);
        return registry;
    }

    public static TechRegistry LoadFromFile(string path)
    {
        var json = File.ReadAllText(path);
        return LoadFromJson(json);
    }

    public static TechRegistry LoadDefault()
    {
        var dir = FindDataDirectory();
        var path = Path.Combine(dir, "techs.json");
        return LoadFromFile(path);
    }

    private static string FindDataDirectory()
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null)
        {
            var dataPath = Path.Combine(dir, "data");
            if (Directory.Exists(dataPath))
                return dataPath;
            dir = Directory.GetParent(dir)?.FullName;
        }
        throw new DirectoryNotFoundException(
            "Could not find 'data/' directory. Ensure it exists at the repository root.");
    }
}
