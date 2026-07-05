namespace Gro.EarthModel;

internal static class CountryData
{
    public static void Add(List<Zone> zones)
    {
        var zonesRoot = ZoneLoader.GetZonesRootPath();
        var countriesDir = Path.Combine(zonesRoot, "countries");
        zones.AddRange(ZoneLoader.LoadFromDirectory(countriesDir));
    }
}
