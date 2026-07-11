using System.Text.Json;

namespace Gro.EarthModel;

public sealed class AdjacencyMap
{
    private readonly Dictionary<string, HashSet<string>> _neighbors = new();
    private readonly Dictionary<(string, string), int> _borderLengths = new();

    private AdjacencyMap() { }

    public static AdjacencyMap LoadFromFile(string path)
    {
        var map = new AdjacencyMap();
        var json = File.ReadAllText(path);
        var entries = JsonSerializer.Deserialize<JsonElement[]>(json)!;
        foreach (var entry in entries)
        {
            var a = entry[0].GetString()!;
            var b = entry[1].GetString()!;
            int lengthKm = entry.GetArrayLength() > 2 ? entry[2].GetInt32() : 0;
            map.AddEdge(a, b, lengthKm);
        }
        return map;
    }

    public static AdjacencyMap LoadFromBordersDirectory()
    {
        var dir = FindBordersDirectory();
        var path = Path.Combine(dir, "adjacency.json");
        return LoadFromFile(path);
    }

    public static AdjacencyMap FromZones(IReadOnlyList<Zone> zones)
    {
        const double Threshold = 1.5;
        var map = new AdjacencyMap();
        var childZones = zones
            .Where(z => z.Type == ZoneType.Country || z.Type == ZoneType.OceanZone)
            .ToArray();

        for (int i = 0; i < childZones.Length; i++)
        {
            for (int j = i + 1; j < childZones.Length; j++)
            {
                if (BoundariesWithinThreshold(childZones[i], childZones[j], Threshold))
                {
                    map.AddEdge(childZones[i].Name, childZones[j].Name, 0);
                }
            }
        }
        return map;
    }

    public IReadOnlySet<string> GetNeighbors(string zoneName)
    {
        if (_neighbors.TryGetValue(zoneName, out var set))
            return set;
        return new HashSet<string>();
    }

    public bool AreAdjacent(string a, string b)
    {
        return _neighbors.TryGetValue(a, out var set) && set.Contains(b);
    }

    public int GetBorderLength(string a, string b)
    {
        var key = string.Compare(a, b, StringComparison.Ordinal) < 0 ? (a, b) : (b, a);
        return _borderLengths.TryGetValue(key, out var length) ? length : 0;
    }

    private static bool BoundariesWithinThreshold(Zone a, Zone b, double threshold)
    {
        foreach (var p in a.Boundary)
        {
            for (int i = 0, j = b.Boundary.Length - 1; i < b.Boundary.Length; j = i++)
            {
                if (PointToSegmentDist(p, b.Boundary[j], b.Boundary[i]) <= threshold)
                    return true;
            }
        }
        foreach (var p in b.Boundary)
        {
            for (int i = 0, j = a.Boundary.Length - 1; i < a.Boundary.Length; j = i++)
            {
                if (PointToSegmentDist(p, a.Boundary[j], a.Boundary[i]) <= threshold)
                    return true;
            }
        }
        return false;
    }

    private static double PointToSegmentDist(GeoCoord p, GeoCoord a, GeoCoord b)
    {
        double dx = b.Lon - a.Lon;
        double dy = b.Lat - a.Lat;
        double lenSq = dx * dx + dy * dy;
        if (lenSq == 0)
            return Math.Sqrt((p.Lon - a.Lon) * (p.Lon - a.Lon) + (p.Lat - a.Lat) * (p.Lat - a.Lat));

        double t = Math.Clamp(((p.Lon - a.Lon) * dx + (p.Lat - a.Lat) * dy) / lenSq, 0, 1);
        double nx = a.Lon + t * dx;
        double ny = a.Lat + t * dy;
        return Math.Sqrt((p.Lon - nx) * (p.Lon - nx) + (p.Lat - ny) * (p.Lat - ny));
    }

    private void AddEdge(string a, string b, int lengthKm)
    {
        if (!_neighbors.TryGetValue(a, out var setA))
        {
            setA = new HashSet<string>();
            _neighbors[a] = setA;
        }
        setA.Add(b);

        if (!_neighbors.TryGetValue(b, out var setB))
        {
            setB = new HashSet<string>();
            _neighbors[b] = setB;
        }
        setB.Add(a);

        var key = string.Compare(a, b, StringComparison.Ordinal) < 0 ? (a, b) : (b, a);
        _borderLengths[key] = lengthKm;
    }

    private static string FindBordersDirectory()
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null)
        {
            var bordersPath = Path.Combine(dir, "borders");
            if (Directory.Exists(bordersPath))
                return bordersPath;
            dir = Directory.GetParent(dir)?.FullName;
        }
        throw new DirectoryNotFoundException(
            "Could not find 'borders/' directory. Ensure it exists at the repository root.");
    }
}
