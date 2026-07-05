using Gro.EarthModel;

var earth = Earth.Create();
var topLevelZones = earth.Zones
    .Where(z => z.Type == ZoneType.Continent || z.Type == ZoneType.OceanBasin)
    .ToList();
var allZones = earth.Zones;

string? filterPrefix = args.Length > 0 ? args[0] : null;
bool verbose = args.Contains("--verbose");

int passCount = 0;
int failCount = 0;
var failures = new List<string>();

void Pass(string msg)
{
    passCount++;
    if (verbose)
        Console.WriteLine($"  PASS: {msg}");
}

void Fail(string msg)
{
    failCount++;
    failures.Add(msg);
    Console.WriteLine($"  FAIL: {msg}");
}

// --- Check 1: Per-zone overlap detection (each top-level zone vs others) ---
Console.WriteLine("=== Top-Level Zone Overlap Checks ===");
foreach (var zone in topLevelZones)
{
    if (filterPrefix != null && !zone.Name.StartsWith(filterPrefix, StringComparison.OrdinalIgnoreCase))
        continue;

    Console.WriteLine($"[{zone.Name}]");
    var others = topLevelZones.Where(z => z != zone).ToList();
    var overlapPoints = new List<(GeoCoord Point, string OtherZone)>();

    var centroid = zone.Centroid;
    double minLat = zone.Boundary.Min(b => b.Lat) - 1;
    double maxLat = zone.Boundary.Max(b => b.Lat) + 1;
    double minLon = zone.Boundary.Min(b => b.Lon) - 1;
    double maxLon = zone.Boundary.Max(b => b.Lon) + 1;

    for (double lat = minLat; lat <= maxLat; lat += 5)
    {
        for (double lon = minLon; lon <= maxLon; lon += 5)
        {
            var point = new GeoCoord(lat, lon);
            if (!zone.Contains(point))
                continue;

            foreach (var other in others)
            {
                if (other.Contains(point))
                    overlapPoints.Add((point, other.Name));
            }
        }
    }

    if (overlapPoints.Count == 0)
        Pass("No overlaps found");
    else
    {
        var grouped = overlapPoints.GroupBy(o => o.OtherZone);
        foreach (var g in grouped)
            Fail($"Overlaps with [{g.Key}] at {g.Count()} point(s): {string.Join(", ", g.Take(3).Select(p => $"({p.Point.Lat},{p.Point.Lon})"))}");
    }
}

// --- Check 2: Per-zone coverage (sample points inside each zone's bounding box) ---
Console.WriteLine("\n=== Top-Level Zone Coverage Checks ===");
for (double lat = -89; lat <= 89; lat += 5)
{
    for (double lon = -179; lon <= 179; lon += 5)
    {
        var point = new GeoCoord(lat, lon);
        var containing = topLevelZones.Where(z => z.Contains(point)).ToList();
        if (containing.Count == 0)
        {
            if (filterPrefix == null || $"({lat},{lon})".Contains(filterPrefix))
                Fail($"Uncovered point at ({lat},{lon})");
        }
    }
}
if (failCount == 0 || failures.All(f => f.StartsWith("Overlaps")))
    Pass("All sampled points are covered by at least one top-level zone");

// --- Check 3: Child zone hierarchy validation ---
Console.WriteLine("\n=== Child Zone Hierarchy Checks ===");
foreach (var zone in allZones)
{
    if (zone.ParentName == null)
        continue;
    if (filterPrefix != null && !zone.Name.StartsWith(filterPrefix, StringComparison.OrdinalIgnoreCase))
        continue;

    var parent = earth.FindZone(zone.ParentName);
    if (parent == null)
        Fail($"[{zone.Name}] references non-existent parent '{zone.ParentName}'");
    else
        Pass($"[{zone.Name}] parent '{zone.ParentName}' exists");
}

// --- Check 4: Capital city containment (sequential per zone) ---
Console.WriteLine("\n=== Capital City Containment Checks ===");
var capitalTests = new (double Lat, double Lon, string Zone)[]
{
    (48.85, 2.35, "France"), (52.52, 13.40, "Germany"), (51.5, -0.12, "United Kingdom"),
    (40.42, -3.70, "Spain"), (41.9, 12.5, "Italy"), (52.23, 21.0, "Poland"),
    (50.45, 30.5, "Ukraine"), (59.33, 18.07, "Sweden"), (60.0, 10.75, "Norway"),
    (55.75, 37.6, "Russia (European)"), (64.15, -21.9, "Iceland"), (53.35, -6.26, "Ireland"),
    (38.72, -9.14, "Portugal"), (60.17, 24.94, "Finland"), (55.68, 12.57, "Denmark"),
    (52.37, 4.9, "Netherlands"), (50.85, 4.35, "Belgium"), (46.95, 7.45, "Switzerland"),
    (48.21, 16.37, "Austria"), (50.08, 14.44, "Czech Republic"), (48.15, 17.11, "Slovakia"),
    (47.5, 19.04, "Hungary"), (44.43, 26.1, "Romania"), (42.7, 23.32, "Bulgaria"),
    (37.98, 23.73, "Greece"), (44.8, 20.47, "Serbia"), (45.81, 15.98, "Croatia"),
    (43.85, 18.36, "Bosnia and Herzegovina"), (42.44, 19.26, "Montenegro"),
    (41.0, 21.43, "North Macedonia"), (41.33, 19.82, "Albania"), (46.05, 14.51, "Slovenia"),
    (53.9, 27.57, "Belarus"), (47.01, 28.86, "Moldova"), (59.44, 24.75, "Estonia"),
    (56.95, 24.11, "Latvia"), (54.69, 25.28, "Lithuania"),
    // Africa
    (34.02, -6.84, "Morocco"), (36.75, 3.06, "Algeria"), (36.81, 10.17, "Tunisia"),
    (32.9, 13.18, "Libya"), (30.04, 31.24, "Egypt"), (18.09, -15.98, "Mauritania"),
    (12.65, -8.0, "Mali"), (13.51, 2.11, "Niger"), (14.69, -17.44, "Senegal"),
    (13.45, -16.58, "Gambia"), (11.86, -15.6, "Guinea-Bissau"), (9.64, -13.58, "Guinea"),
    (8.48, -13.23, "Sierra Leone"), (6.3, -10.8, "Liberia"), (6.82, -5.28, "Ivory Coast"),
    (12.37, -1.52, "Burkina Faso"), (5.56, -0.19, "Ghana"), (6.13, 1.22, "Togo"),
    (6.49, 2.6, "Benin"), (9.06, 7.49, "Nigeria"), (12.13, 15.05, "Chad"),
    (3.87, 11.52, "Cameroon"), (4.36, 18.56, "Central African Republic"),
    (3.75, 8.78, "Equatorial Guinea"), (0.39, 9.45, "Gabon"),
    (-4.27, 15.28, "Republic of Congo"), (-4.32, 15.31, "DR Congo"),
    (15.59, 32.53, "Sudan"), (4.85, 31.6, "South Sudan"), (15.34, 38.93, "Eritrea"),
    (11.59, 43.15, "Djibouti"), (9.02, 38.75, "Ethiopia"), (2.05, 45.32, "Somalia"),
    (-1.29, 36.82, "Kenya"), (0.31, 32.58, "Uganda"), (-1.94, 29.87, "Rwanda"),
    (-3.38, 29.36, "Burundi"), (-6.79, 39.28, "Tanzania"), (-8.84, 13.23, "Angola"),
    (-15.39, 28.32, "Zambia"), (-13.96, 33.79, "Malawi"), (-25.97, 32.57, "Mozambique"),
    (-17.83, 31.05, "Zimbabwe"), (-22.56, 17.08, "Namibia"), (-24.65, 25.91, "Botswana"),
    (-33.93, 18.42, "South Africa"), (-29.31, 27.48, "Lesotho"), (-26.31, 31.13, "Eswatini"),
    (-18.91, 47.52, "Madagascar"),
    // Central America
    (25.67, -100.31, "Mexico (North)"), (19.43, -99.13, "Mexico (Central)"),
    (17.06, -96.72, "Mexico (South)"), (14.63, -90.51, "Guatemala-Belize"),
    (14.1, -87.22, "Honduras-El Salvador-Nicaragua"), (12.15, -86.27, "Honduras-El Salvador-Nicaragua"),
    (9.93, -84.08, "Costa Rica-Panama"), (8.98, -79.52, "Costa Rica-Panama"),
    // Asia
    (55.0, 73.4, "Russia (West Siberia)"), (52.3, 104.3, "Russia (East Siberia)"),
    (43.1, 131.9, "Russia (Far East)"), (39.9, 116.4, "China (North)"),
    (31.2, 121.5, "China (South)"), (43.8, 87.6, "China (West)"),
    (47.9, 106.9, "Mongolia"), (35.7, 139.7, "Japan"), (37.6, 127.0, "South Korea"),
    (39.0, 125.8, "North Korea"), (25.0, 121.5, "Taiwan"), (21.0, 105.8, "Vietnam"),
    (13.75, 100.5, "Thailand"), (16.87, 96.2, "Myanmar"), (11.55, 104.92, "Cambodia"),
    (17.97, 102.63, "Laos"), (3.14, 101.69, "Malaysia"), (14.6, 121.0, "Philippines"),
    (-6.2, 106.8, "Indonesia"), (28.6, 77.2, "India"), (33.7, 73.0, "Pakistan"),
    (23.8, 90.4, "Bangladesh"), (6.93, 79.85, "Sri Lanka"), (27.7, 85.3, "Nepal"),
    (27.5, 89.6, "Bhutan"), (34.5, 69.2, "Afghanistan"), (51.17, 71.45, "Kazakhstan"),
    (41.3, 69.3, "Uzbekistan"), (37.95, 58.38, "Turkmenistan"), (42.87, 74.59, "Kyrgyzstan"),
    (38.56, 68.77, "Tajikistan"), (39.93, 32.86, "Turkey"), (24.7, 46.7, "Saudi Arabia"),
    (35.7, 51.4, "Iran"), (33.3, 44.4, "Iraq"), (33.5, 36.3, "Syria"),
    (31.95, 35.93, "Jordan"), (33.89, 35.5, "Lebanon-Israel"), (15.35, 44.21, "Yemen"),
    (23.6, 58.5, "Oman"), (25.3, 51.5, "UAE-Qatar-Kuwait-Bahrain"),
    (41.72, 44.78, "Georgia-Armenia-Azerbaijan"),
    // Oceania
    (-33.87, 151.21, "Australia"), (-41.29, 174.78, "New Zealand"),
    (-6.21, 155.96, "Solomon Islands"), (-6.31, 147.15, "Papua New Guinea"),
    (-17.73, 168.32, "Vanuatu-New Caledonia"), (-17.77, 177.97, "Fiji"),
    (-13.83, -171.76, "Samoa-Tonga"), (7.09, 171.38, "Micronesia"),
    (-17.53, -149.57, "French Polynesia"),
};

foreach (var (lat, lon, zoneName) in capitalTests)
{
    if (filterPrefix != null && !zoneName.StartsWith(filterPrefix, StringComparison.OrdinalIgnoreCase))
        continue;

    var zone = earth.FindZone(zoneName);
    var point = new GeoCoord(lat, lon);

    if (zone == null)
    {
        Fail($"[{zoneName}] zone not found");
        continue;
    }

    if (zone.Contains(point))
        Pass($"[{zoneName}] contains ({lat},{lon})");
    else
        Fail($"[{zoneName}] does NOT contain its capital at ({lat},{lon})");
}

// --- Summary ---
Console.WriteLine($"\n=== Summary ===");
Console.WriteLine($"Passed: {passCount}");
Console.WriteLine($"Failed: {failCount}");

if (failures.Count > 0)
{
    Console.WriteLine($"\nAll failures:");
    foreach (var f in failures)
        Console.WriteLine($"  - {f}");
}

return failCount > 0 ? 1 : 0;
