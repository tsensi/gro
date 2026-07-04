using Gro.EarthModel;

namespace Gro.Tests;

public class EarthModelTests
{
    private readonly Earth _earth = Earth.Create();

    [Fact]
    public void Create_HasContinents()
    {
        var continents = _earth.GetByType(ZoneType.Continent).ToList();
        Assert.Equal(7, continents.Count);
        Assert.Contains(continents, c => c.Name == "North America");
        Assert.Contains(continents, c => c.Name == "South America");
        Assert.Contains(continents, c => c.Name == "Europe");
        Assert.Contains(continents, c => c.Name == "Africa");
        Assert.Contains(continents, c => c.Name == "Asia");
        Assert.Contains(continents, c => c.Name == "Oceania");
        Assert.Contains(continents, c => c.Name == "Antarctica");
    }

    [Fact]
    public void Create_HasOceanBasins()
    {
        var basins = _earth.GetByType(ZoneType.OceanBasin).ToList();
        Assert.Equal(5, basins.Count);
        Assert.Contains(basins, b => b.Name == "Atlantic Ocean");
        Assert.Contains(basins, b => b.Name == "Pacific Ocean");
        Assert.Contains(basins, b => b.Name == "Indian Ocean");
        Assert.Contains(basins, b => b.Name == "Arctic Ocean");
        Assert.Contains(basins, b => b.Name == "Southern Ocean");
    }

    [Fact]
    public void Create_HasCountries()
    {
        var countries = _earth.GetByType(ZoneType.Country).ToList();
        Assert.True(countries.Count >= 25);
    }

    [Fact]
    public void Create_HasOceanZones()
    {
        var zones = _earth.GetByType(ZoneType.OceanZone).ToList();
        Assert.True(zones.Count >= 20);
        Assert.Contains(zones, z => z.Name == "Eastern Mediterranean");
        Assert.Contains(zones, z => z.Name == "Caribbean Sea");
        Assert.Contains(zones, z => z.Name == "Arabian Sea");
    }

    [Fact]
    public void GetChildren_ReturnsCountriesForContinent()
    {
        var naChildren = _earth.GetChildren("North America").ToList();
        Assert.Contains(naChildren, c => c.Name == "Canada");
        Assert.Contains(naChildren, c => c.Name == "United States");
        Assert.Contains(naChildren, c => c.Name == "Mexico");
    }

    [Fact]
    public void GetChildren_ReturnsOceanZonesForBasin()
    {
        var atlanticZones = _earth.GetChildren("Atlantic Ocean").ToList();
        Assert.Contains(atlanticZones, z => z.Name == "Caribbean Sea");
        Assert.Contains(atlanticZones, z => z.Name == "Mediterranean Sea");
        Assert.Contains(atlanticZones, z => z.Name == "North Atlantic");
    }

    [Fact]
    public void FindZone_ReturnsCorrectZone()
    {
        var brazil = _earth.FindZone("Brazil");
        Assert.NotNull(brazil);
        Assert.Equal(ZoneType.Country, brazil.Type);
        Assert.Equal("South America", brazil.ParentName);
    }

    [Fact]
    public void FindZone_ReturnsNullForUnknown()
    {
        Assert.Null(_earth.FindZone("Atlantis"));
    }

    [Theory]
    [InlineData(48.86, 2.35, "France")]       // Paris
    [InlineData(40.71, -74.01, "United States")] // NYC
    [InlineData(-33.87, 151.21, "Australia")]  // Sydney
    [InlineData(35.68, 139.69, "Japan")]       // Tokyo
    [InlineData(-15.79, -47.88, "Brazil")]     // Brasilia
    public void ZoneAt_FindsCorrectCountry(double lat, double lon, string expectedCountry)
    {
        var zone = _earth.ZoneAt(new GeoCoord(lat, lon));
        Assert.NotNull(zone);
        Assert.Equal(expectedCountry, zone.Name);
    }

    [Fact]
    public void Zone_Centroid_IsReasonable()
    {
        var france = _earth.FindZone("France")!;
        var centroid = france.Centroid;
        // France centroid should be somewhere near 46N, 2E
        Assert.InRange(centroid.Lat, 42, 50);
        Assert.InRange(centroid.Lon, -2, 8);
    }

    [Fact]
    public void GeoCoord_DistanceKm_KnownDistance()
    {
        var london = new GeoCoord(51.5, -0.12);
        var paris = new GeoCoord(48.86, 2.35);
        double dist = london.DistanceKm(paris);
        // London to Paris is ~340km
        Assert.InRange(dist, 320, 360);
    }

    [Fact]
    public void Hierarchy_AllCountriesHaveValidParent()
    {
        var countries = _earth.GetByType(ZoneType.Country);
        var continentNames = _earth.GetByType(ZoneType.Continent).Select(c => c.Name).ToHashSet();

        foreach (var country in countries)
        {
            Assert.NotNull(country.ParentName);
            Assert.Contains(country.ParentName, continentNames);
        }
    }

    [Fact]
    public void Hierarchy_AllOceanZonesHaveValidParent()
    {
        var zones = _earth.GetByType(ZoneType.OceanZone);
        var basinNames = _earth.GetByType(ZoneType.OceanBasin).Select(b => b.Name).ToHashSet();

        foreach (var zone in zones)
        {
            Assert.NotNull(zone.ParentName);
            Assert.Contains(zone.ParentName, basinNames);
        }
    }

    [Fact]
    public void Zone_Contains_PointInsideIsTrue()
    {
        var us = _earth.FindZone("United States")!;
        // Kansas City
        Assert.True(us.Contains(new GeoCoord(39.1, -94.6)));
    }

    [Fact]
    public void Zone_Contains_PointOutsideIsFalse()
    {
        var us = _earth.FindZone("United States")!;
        // London
        Assert.False(us.Contains(new GeoCoord(51.5, -0.12)));
    }

    [Fact]
    public void ZoneAt_OceanPoint_FindsOceanZone()
    {
        // Mid-Atlantic point
        var zone = _earth.ZoneAt(new GeoCoord(35, -40));
        Assert.NotNull(zone);
        Assert.Equal(ZoneType.OceanZone, zone.Type);
    }

    [Fact]
    public void TotalZoneCount_IsSubstantial()
    {
        Assert.True(_earth.Zones.Count >= 60);
    }
}
