using Gro.EarthModel;

namespace Gro.Tests;

public class ZoneCoverageTests
{
    private readonly Earth _earth = Earth.Create();

    private IReadOnlyList<Zone> TopLevelZones =>
        _earth.Zones.Where(z => z.Type == ZoneType.Continent || z.Type == ZoneType.OceanBasin).ToList();

    [Fact]
    public void TopLevelZones_CoverEntireGlobe()
    {
        var topZones = TopLevelZones;
        var uncovered = new List<GeoCoord>();

        for (double lat = -85; lat <= 85; lat += 10)
        {
            for (double lon = -175; lon <= 175; lon += 10)
            {
                var point = new GeoCoord(lat, lon);
                bool covered = topZones.Any(z => z.Contains(point));
                if (!covered)
                    uncovered.Add(point);
            }
        }

        Assert.True(uncovered.Count == 0,
            $"Found {uncovered.Count} uncovered points. First few: " +
            string.Join(", ", uncovered.Take(10).Select(p => $"({p.Lat},{p.Lon})")));
    }

    [Fact]
    public void TopLevelZones_DoNotOverlap()
    {
        var topZones = TopLevelZones;
        var overlaps = new List<(GeoCoord Point, string Zone1, string Zone2)>();

        for (double lat = -85; lat <= 85; lat += 10)
        {
            for (double lon = -175; lon <= 175; lon += 10)
            {
                var point = new GeoCoord(lat, lon);
                var containing = topZones.Where(z => z.Contains(point)).ToList();
                if (containing.Count > 1)
                {
                    overlaps.Add((point, containing[0].Name, containing[1].Name));
                }
            }
        }

        Assert.True(overlaps.Count == 0,
            $"Found {overlaps.Count} overlapping points. First few: " +
            string.Join(", ", overlaps.Take(10).Select(o => $"({o.Point.Lat},{o.Point.Lon}) in [{o.Zone1}] and [{o.Zone2}]")));
    }

    [Fact]
    public void TopLevelZones_CoverEntireGlobe_FineMesh()
    {
        var topZones = TopLevelZones;
        var uncovered = new List<GeoCoord>();

        for (double lat = -89; lat <= 89; lat += 5)
        {
            for (double lon = -179; lon <= 179; lon += 5)
            {
                var point = new GeoCoord(lat, lon);
                bool covered = topZones.Any(z => z.Contains(point));
                if (!covered)
                    uncovered.Add(point);
            }
        }

        Assert.True(uncovered.Count == 0,
            $"Found {uncovered.Count} uncovered points on fine mesh. First few: " +
            string.Join(", ", uncovered.Take(20).Select(p => $"({p.Lat},{p.Lon})")));
    }

    [Fact]
    public void TopLevelZones_DoNotOverlap_FineMesh()
    {
        var topZones = TopLevelZones;
        var overlaps = new List<(GeoCoord Point, string Zone1, string Zone2)>();

        for (double lat = -89; lat <= 89; lat += 5)
        {
            for (double lon = -179; lon <= 179; lon += 5)
            {
                var point = new GeoCoord(lat, lon);
                var containing = topZones.Where(z => z.Contains(point)).ToList();
                if (containing.Count > 1)
                {
                    overlaps.Add((point, containing[0].Name, containing[1].Name));
                }
            }
        }

        Assert.True(overlaps.Count == 0,
            $"Found {overlaps.Count} overlapping points on fine mesh. First few: " +
            string.Join(", ", overlaps.Take(20).Select(o => $"({o.Point.Lat},{o.Point.Lon}) in [{o.Zone1}] and [{o.Zone2}]")));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(45, 45)]
    [InlineData(-45, -45)]
    [InlineData(80, 100)]
    [InlineData(-80, -100)]
    [InlineData(0, 180)]
    [InlineData(0, -180)]
    [InlineData(89, 0)]
    [InlineData(-89, 0)]
    [InlineData(30, -50)]
    [InlineData(-30, 80)]
    [InlineData(60, -30)]
    public void EveryPoint_IsInExactlyOneTopLevelZone(double lat, double lon)
    {
        var point = new GeoCoord(lat, lon);
        var containing = TopLevelZones.Where(z => z.Contains(point)).ToList();

        Assert.True(containing.Count == 1,
            $"Point ({lat},{lon}) is in {containing.Count} top-level zones: [{string.Join(", ", containing.Select(z => z.Name))}]");
    }

    [Theory]
    [InlineData(48.85, 2.35, "France")]
    [InlineData(52.52, 13.40, "Germany")]
    [InlineData(51.5, -0.12, "United Kingdom")]
    [InlineData(40.42, -3.70, "Spain")]
    [InlineData(41.9, 12.5, "Italy")]
    [InlineData(52.23, 21.0, "Poland")]
    [InlineData(50.45, 30.5, "Ukraine")]
    [InlineData(59.33, 18.07, "Sweden")]
    [InlineData(60.0, 10.75, "Norway")]
    [InlineData(55.75, 37.6, "Russia (European)")]
    [InlineData(64.15, -21.9, "Iceland")]
    [InlineData(53.35, -6.26, "Ireland")]
    [InlineData(38.72, -9.14, "Portugal")]
    [InlineData(60.17, 24.94, "Finland")]
    [InlineData(55.68, 12.57, "Denmark")]
    [InlineData(52.37, 4.9, "Netherlands")]
    [InlineData(50.85, 4.35, "Belgium")]
    [InlineData(46.95, 7.45, "Switzerland")]
    [InlineData(48.21, 16.37, "Austria")]
    [InlineData(50.08, 14.44, "Czech Republic")]
    [InlineData(48.15, 17.11, "Slovakia")]
    [InlineData(47.5, 19.04, "Hungary")]
    [InlineData(44.43, 26.1, "Romania")]
    [InlineData(42.7, 23.32, "Bulgaria")]
    [InlineData(37.98, 23.73, "Greece")]
    [InlineData(44.8, 20.47, "Serbia")]
    [InlineData(45.81, 15.98, "Croatia")]
    [InlineData(43.85, 18.36, "Bosnia and Herzegovina")]
    [InlineData(42.44, 19.26, "Montenegro")]
    [InlineData(41.0, 21.43, "North Macedonia")]
    [InlineData(41.33, 19.82, "Albania")]
    [InlineData(46.05, 14.51, "Slovenia")]
    [InlineData(53.9, 27.57, "Belarus")]
    [InlineData(47.01, 28.86, "Moldova")]
    [InlineData(59.44, 24.75, "Estonia")]
    [InlineData(56.95, 24.11, "Latvia")]
    [InlineData(54.69, 25.28, "Lithuania")]
    public void EuropeanCapital_IsInCorrectCountryZone(double lat, double lon, string expectedCountry)
    {
        var point = new GeoCoord(lat, lon);
        var zone = _earth.FindZone(expectedCountry);

        Assert.NotNull(zone);
        Assert.True(zone!.Contains(point),
            $"Zone '{expectedCountry}' does not contain its capital at ({lat},{lon})");
    }

    [Theory]
    [InlineData(34.02, -6.84, "Morocco")]
    [InlineData(36.75, 3.06, "Algeria")]
    [InlineData(36.81, 10.17, "Tunisia")]
    [InlineData(32.9, 13.18, "Libya")]
    [InlineData(30.04, 31.24, "Egypt")]
    [InlineData(18.09, -15.98, "Mauritania")]
    [InlineData(12.65, -8.0, "Mali")]
    [InlineData(13.51, 2.11, "Niger")]
    [InlineData(14.69, -17.44, "Senegal")]
    [InlineData(13.45, -16.58, "Gambia")]
    [InlineData(11.86, -15.6, "Guinea-Bissau")]
    [InlineData(9.64, -13.58, "Guinea")]
    [InlineData(8.48, -13.23, "Sierra Leone")]
    [InlineData(6.3, -10.8, "Liberia")]
    [InlineData(6.82, -5.28, "Ivory Coast")]
    [InlineData(12.37, -1.52, "Burkina Faso")]
    [InlineData(5.56, -0.19, "Ghana")]
    [InlineData(6.13, 1.22, "Togo")]
    [InlineData(6.49, 2.6, "Benin")]
    [InlineData(9.06, 7.49, "Nigeria")]
    [InlineData(12.13, 15.05, "Chad")]
    [InlineData(3.87, 11.52, "Cameroon")]
    [InlineData(4.36, 18.56, "Central African Republic")]
    [InlineData(3.75, 8.78, "Equatorial Guinea")]
    [InlineData(0.39, 9.45, "Gabon")]
    [InlineData(-4.27, 15.28, "Republic of Congo")]
    [InlineData(-4.32, 15.31, "DR Congo")]
    [InlineData(15.59, 32.53, "Sudan")]
    [InlineData(4.85, 31.6, "South Sudan")]
    [InlineData(15.34, 38.93, "Eritrea")]
    [InlineData(11.59, 43.15, "Djibouti")]
    [InlineData(9.02, 38.75, "Ethiopia")]
    [InlineData(2.05, 45.32, "Somalia")]
    [InlineData(-1.29, 36.82, "Kenya")]
    [InlineData(0.31, 32.58, "Uganda")]
    [InlineData(-1.94, 29.87, "Rwanda")]
    [InlineData(-3.38, 29.36, "Burundi")]
    [InlineData(-6.79, 39.28, "Tanzania")]
    [InlineData(-8.84, 13.23, "Angola")]
    [InlineData(-15.39, 28.32, "Zambia")]
    [InlineData(-13.96, 33.79, "Malawi")]
    [InlineData(-25.97, 32.57, "Mozambique")]
    [InlineData(-17.83, 31.05, "Zimbabwe")]
    [InlineData(-22.56, 17.08, "Namibia")]
    [InlineData(-24.65, 25.91, "Botswana")]
    [InlineData(-33.93, 18.42, "South Africa")]
    [InlineData(-29.31, 27.48, "Lesotho")]
    [InlineData(-26.31, 31.13, "Eswatini")]
    [InlineData(-18.91, 47.52, "Madagascar")]
    public void AfricanCapital_IsInCorrectCountryZone(double lat, double lon, string expectedCountry)
    {
        var point = new GeoCoord(lat, lon);
        var zone = _earth.FindZone(expectedCountry);

        Assert.NotNull(zone);
        Assert.True(zone!.Contains(point),
            $"Zone '{expectedCountry}' does not contain its capital at ({lat},{lon})");
    }

    [Theory]
    [InlineData(25.67, -100.31, "Mexico (North)")]       // Monterrey
    [InlineData(19.43, -99.13, "Mexico (Central)")]      // Mexico City
    [InlineData(17.06, -96.72, "Mexico (South)")]        // Oaxaca
    [InlineData(14.63, -90.51, "Guatemala-Belize")]      // Guatemala City
    [InlineData(14.1, -87.22, "Honduras-El Salvador-Nicaragua")] // Tegucigalpa
    [InlineData(12.15, -86.27, "Honduras-El Salvador-Nicaragua")] // Managua
    [InlineData(9.93, -84.08, "Costa Rica-Panama")]      // San Jose
    [InlineData(8.98, -79.52, "Costa Rica-Panama")]      // Panama City
    public void CentralAmericanCity_IsInCorrectZone(double lat, double lon, string expectedCountry)
    {
        var point = new GeoCoord(lat, lon);
        var zone = _earth.FindZone(expectedCountry);

        Assert.NotNull(zone);
        Assert.True(zone!.Contains(point),
            $"Zone '{expectedCountry}' does not contain point at ({lat},{lon})");
    }
}
