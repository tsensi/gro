namespace Gro.EarthModel;

public sealed class Earth
{
    public IReadOnlyList<Zone> Zones { get; }

    private Earth(List<Zone> zones)
    {
        Zones = zones;
    }

    public IEnumerable<Zone> GetChildren(string parentName) =>
        Zones.Where(z => z.ParentName == parentName);

    public IEnumerable<Zone> GetByType(ZoneType type) =>
        Zones.Where(z => z.Type == type);

    public Zone? FindZone(string name) =>
        Zones.FirstOrDefault(z => z.Name == name);

    public Zone? ZoneAt(GeoCoord point)
    {
        // Search from most specific to least specific
        foreach (var type in new[] { ZoneType.Country, ZoneType.OceanZone, ZoneType.Continent, ZoneType.OceanBasin })
        {
            foreach (var zone in Zones.Where(z => z.Type == type))
            {
                if (zone.Contains(point))
                    return zone;
            }
        }
        return null;
    }

    public static Earth Create()
    {
        var zones = new List<Zone>();
        ContinentData.Add(zones);
        CountryData.Add(zones);
        OceanData.Add(zones);
        return new Earth(zones);
    }
}
