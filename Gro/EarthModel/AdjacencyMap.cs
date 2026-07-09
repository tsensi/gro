namespace Gro.EarthModel;

public sealed class AdjacencyMap
{
    private const double Threshold = 1.5;

    private readonly Dictionary<string, HashSet<string>> _neighbors = new();

    public AdjacencyMap(IReadOnlyList<Zone> zones)
    {
        var childZones = zones
            .Where(z => z.Type == ZoneType.Country || z.Type == ZoneType.OceanZone)
            .ToArray();

        for (int i = 0; i < childZones.Length; i++)
        {
            for (int j = i + 1; j < childZones.Length; j++)
            {
                if (BoundariesWithinThreshold(childZones[i], childZones[j]))
                {
                    AddEdge(childZones[i].Name, childZones[j].Name);
                }
            }
        }
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

    private static bool BoundariesWithinThreshold(Zone a, Zone b)
    {
        foreach (var p in a.Boundary)
        {
            for (int i = 0, j = b.Boundary.Length - 1; i < b.Boundary.Length; j = i++)
            {
                if (PointToSegmentDist(p, b.Boundary[j], b.Boundary[i]) <= Threshold)
                    return true;
            }
        }
        foreach (var p in b.Boundary)
        {
            for (int i = 0, j = a.Boundary.Length - 1; i < a.Boundary.Length; j = i++)
            {
                if (PointToSegmentDist(p, a.Boundary[j], a.Boundary[i]) <= Threshold)
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

    private void AddEdge(string a, string b)
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
    }
}
