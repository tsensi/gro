namespace Gro.EarthModel;

internal static class ContinentData
{
    public static void Add(List<Zone> zones)
    {
        var zonesRoot = ZoneLoader.GetZonesRootPath();
        var continentsDir = Path.Combine(zonesRoot, "continents");
        zones.AddRange(ZoneLoader.LoadFromDirectory(continentsDir));
    }
}
