namespace Gro.EarthModel;

internal static class OceanData
{
    public static void Add(List<Zone> zones)
    {
        var zonesRoot = ZoneLoader.GetZonesRootPath();
        var oceansDir = Path.Combine(zonesRoot, "oceans");
        zones.AddRange(ZoneLoader.LoadFromDirectory(oceansDir));
    }
}
