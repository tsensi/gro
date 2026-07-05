using System.Text.Json;

namespace Gro.EarthModel;

public static class ZoneLoader
{
    public static List<Zone> LoadFromDirectory(string directoryPath)
    {
        var zones = new List<Zone>();
        foreach (var file in Directory.GetFiles(directoryPath, "*.json"))
        {
            var zone = LoadFromFile(file);
            zones.Add(zone);
        }
        return zones;
    }

    public static Zone LoadFromFile(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var name = root.GetProperty("name").GetString()!;
        var typeStr = root.GetProperty("type").GetString()!;
        var type = Enum.Parse<ZoneType>(typeStr);
        string? parent = root.GetProperty("parent").ValueKind == JsonValueKind.Null
            ? null
            : root.GetProperty("parent").GetString();

        var boundaryArray = root.GetProperty("boundary");
        var boundary = new GeoCoord[boundaryArray.GetArrayLength() - 1]; // skip closing point
        for (int i = 0; i < boundary.Length; i++)
        {
            var pair = boundaryArray[i];
            double lon = pair[0].GetDouble();
            double lat = pair[1].GetDouble();
            boundary[i] = new GeoCoord(lat, lon);
        }

        return new Zone
        {
            Name = name,
            Type = type,
            ParentName = parent,
            Boundary = boundary,
        };
    }

    public static string GetZonesRootPath()
    {
        var dir = AppContext.BaseDirectory;
        // Walk up from bin/Debug/net8.0 to find the zones/ directory
        while (dir != null)
        {
            var zonesPath = Path.Combine(dir, "zones");
            if (Directory.Exists(zonesPath))
                return zonesPath;
            dir = Directory.GetParent(dir)?.FullName;
        }
        throw new DirectoryNotFoundException(
            "Could not find 'zones/' directory. Ensure it exists at the repository root.");
    }
}
