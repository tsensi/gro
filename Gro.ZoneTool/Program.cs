using System.Text.Json;
using Gro.EarthModel;

if (args.Length == 0)
{
    PrintUsage();
    return 1;
}

var command = args[0].ToLowerInvariant();
var remainingArgs = args.Skip(1).ToArray();

return command switch
{
    "validate" => RunValidate(remainingArgs),
    "normalize" => RunNormalize(remainingArgs),
    "subtract" => RunSubtract(remainingArgs),
    "split-difference" => RunSplitDifference(remainingArgs),
    "adjacency" => RunAdjacency(remainingArgs),
    _ => PrintUsage()
};

static int PrintUsage()
{
    Console.WriteLine("Gro.ZoneTool — validate and normalize zone JSON files");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project Gro.ZoneTool -- validate [path|--all]");
    Console.WriteLine("  dotnet run --project Gro.ZoneTool -- normalize [path|--all] [--dry-run]");
    Console.WriteLine("  dotnet run --project Gro.ZoneTool -- subtract <subject.json> <clip.json> [--dry-run]");
    Console.WriteLine("  dotnet run --project Gro.ZoneTool -- split-difference <zone1.json> <zone2.json> [--dry-run]");
    Console.WriteLine("  dotnet run --project Gro.ZoneTool -- adjacency [--csv] [--compare <reference.csv>]");
    Console.WriteLine();
    Console.WriteLine("Commands:");
    Console.WriteLine("  validate    Check zone files against the schema and validation rules");
    Console.WriteLine("  normalize   Rewrite zone files with correct rounding, closure, and formatting");
    Console.WriteLine("  subtract    Subtract clip polygon from subject polygon, writing result to subject");
    Console.WriteLine("  split-difference  Split the intersection of two polygons along the median line");
    Console.WriteLine("  adjacency   Export computed zone adjacency as CSV (same format as borders/reference.csv)");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  path        Path to a specific .json file or directory");
    Console.WriteLine("  --all       Process all zone files (default if no path given)");
    Console.WriteLine("  --dry-run   Show what would change without writing");
    Console.WriteLine("  --csv       Output adjacency as CSV to stdout");
    Console.WriteLine("  --compare   Compare game adjacency against a reference CSV file");
    return 1;
}

static int RunAdjacency(string[] args)
{
    bool csvMode = args.Contains("--csv");
    string? comparePath = null;
    for (int i = 0; i < args.Length - 1; i++)
    {
        if (args[i] == "--compare")
        {
            comparePath = args[i + 1];
            break;
        }
    }

    var earth = Earth.Create();
    var adjacency = AdjacencyMap.FromZones(earth.Zones);
    var countryZones = earth.Zones
        .Where(z => z.Type == ZoneType.Country)
        .OrderBy(z => z.Name)
        .ToList();

    var pairs = new List<(string A, string B)>();
    for (int i = 0; i < countryZones.Count; i++)
    {
        var neighbors = adjacency.GetNeighbors(countryZones[i].Name);
        foreach (var neighbor in neighbors.OrderBy(n => n))
        {
            var neighborZone = earth.FindZone(neighbor);
            if (neighborZone == null || neighborZone.Type != ZoneType.Country)
                continue;
            var pair = string.Compare(countryZones[i].Name, neighbor, StringComparison.Ordinal) < 0
                ? (countryZones[i].Name, neighbor)
                : (neighbor, countryZones[i].Name);
            pairs.Add(pair);
        }
    }

    pairs = pairs.Distinct().OrderBy(p => p.A).ThenBy(p => p.B).ToList();

    if (comparePath != null)
        return RunCompare(pairs, comparePath);

    if (csvMode)
    {
        Console.WriteLine("country_a,country_b,length_km,type");
        foreach (var (a, b) in pairs)
            Console.WriteLine($"{CsvEscape(a)},{CsvEscape(b)},0,geometric");
    }
    else
    {
        Console.WriteLine($"Game adjacency: {pairs.Count} country pairs");
        Console.WriteLine();
        foreach (var (a, b) in pairs)
            Console.WriteLine($"  {a} <-> {b}");
    }

    return 0;
}

static string CsvEscape(string value)
{
    if (value.Contains(',') || value.Contains('"'))
        return $"\"{value.Replace("\"", "\"\"")}\"";
    return value;
}

static int RunCompare(List<(string A, string B)> gamePairs, string referencePath)
{
    if (!File.Exists(referencePath))
    {
        Console.WriteLine($"ERROR: Reference file not found: {referencePath}");
        return 1;
    }

    var refPairs = new Dictionary<(string, string), (int Length, string Type)>();
    var lines = File.ReadAllLines(referencePath);
    foreach (var line in lines.Skip(1))
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;
        var parts = ParseCsvLine(line);
        if (parts.Length < 4)
            continue;
        var a = parts[0].Trim();
        var b = parts[1].Trim();
        int.TryParse(parts[2].Trim(), out int length);
        var type = parts[3].Trim();
        var pair = string.Compare(a, b, StringComparison.Ordinal) < 0 ? (a, b) : (b, a);
        refPairs[pair] = (length, type);
    }

    var gameSet = new HashSet<(string, string)>(gamePairs);
    var refSet = new HashSet<(string, string)>(refPairs.Keys);

    var onlyInGame = gameSet.Except(refSet).OrderBy(p => p.Item1).ThenBy(p => p.Item2).ToList();
    var onlyInRef = refSet.Except(gameSet).OrderBy(p => p.Item1).ThenBy(p => p.Item2).ToList();
    var inBoth = gameSet.Intersect(refSet).OrderBy(p => p.Item1).ThenBy(p => p.Item2).ToList();

    Console.WriteLine($"=== Adjacency Comparison ===");
    Console.WriteLine($"Game pairs:      {gamePairs.Count}");
    Console.WriteLine($"Reference pairs: {refPairs.Count}");
    Console.WriteLine($"In both:         {inBoth.Count}");
    Console.WriteLine();

    if (onlyInGame.Count > 0)
    {
        Console.WriteLine($"Only in game ({onlyInGame.Count}):");
        foreach (var (a, b) in onlyInGame)
            Console.WriteLine($"  + {a} <-> {b}");
        Console.WriteLine();
    }

    if (onlyInRef.Count > 0)
    {
        Console.WriteLine($"Only in reference ({onlyInRef.Count}):");
        foreach (var (a, b) in onlyInRef)
        {
            var info = refPairs[(a, b)];
            Console.WriteLine($"  - {a} <-> {b} ({info.Length} km, {info.Type})");
        }
    }

    return 0;
}

static string[] ParseCsvLine(string line)
{
    var result = new List<string>();
    bool inQuotes = false;
    var current = new System.Text.StringBuilder();
    foreach (char c in line)
    {
        if (c == '"')
        {
            inQuotes = !inQuotes;
        }
        else if (c == ',' && !inQuotes)
        {
            result.Add(current.ToString());
            current.Clear();
        }
        else
        {
            current.Append(c);
        }
    }
    result.Add(current.ToString());
    return result.ToArray();
}

static List<string> ResolveFiles(string[] args)
{
    var files = new List<string>();
    var paths = args.Where(a => !a.StartsWith("--")).ToList();

    if (paths.Count == 0 || args.Contains("--all"))
    {
        var zonesRoot = ZoneLoader.GetZonesRootPath();
        files.AddRange(Directory.GetFiles(zonesRoot, "*.json", SearchOption.AllDirectories));
    }
    else
    {
        foreach (var path in paths)
        {
            if (Directory.Exists(path))
                files.AddRange(Directory.GetFiles(path, "*.json", SearchOption.AllDirectories));
            else if (File.Exists(path))
                files.Add(path);
            else
                Console.WriteLine($"WARNING: '{path}' not found, skipping");
        }
    }

    files.Sort(StringComparer.OrdinalIgnoreCase);
    return files;
}

static int RunValidate(string[] args)
{
    var files = ResolveFiles(args);
    if (files.Count == 0)
    {
        Console.WriteLine("No zone files found.");
        return 1;
    }

    int totalErrors = 0;
    int filesChecked = 0;
    var validTypes = new HashSet<string> { "Continent", "Country", "OceanBasin", "OceanZone" };
    var topLevelTypes = new HashSet<string> { "Continent", "OceanBasin" };
    var childTypes = new HashSet<string> { "Country", "OceanZone" };

    foreach (var file in files)
    {
        var errors = ValidateFile(file, validTypes, topLevelTypes, childTypes);
        filesChecked++;

        if (errors.Count > 0)
        {
            Console.WriteLine($"FAIL: {RelativePath(file)}");
            foreach (var err in errors)
                Console.WriteLine($"  - {err}");
            totalErrors += errors.Count;
        }
    }

    Console.WriteLine();
    Console.WriteLine($"Checked {filesChecked} files, {totalErrors} error(s) found.");
    return totalErrors > 0 ? 1 : 0;
}

static List<string> ValidateFile(string filePath, HashSet<string> validTypes,
    HashSet<string> topLevelTypes, HashSet<string> childTypes)
{
    var errors = new List<string>();

    string json;
    try
    {
        json = File.ReadAllText(filePath);
    }
    catch (Exception ex)
    {
        errors.Add($"Cannot read file: {ex.Message}");
        return errors;
    }

    JsonDocument doc;
    try
    {
        doc = JsonDocument.Parse(json);
    }
    catch (JsonException ex)
    {
        errors.Add($"Invalid JSON: {ex.Message}");
        return errors;
    }

    var root = doc.RootElement;

    // Check required fields exist
    if (!root.TryGetProperty("name", out var nameProp) || nameProp.ValueKind != JsonValueKind.String)
        errors.Add("Missing or invalid 'name' field (must be a string)");

    if (!root.TryGetProperty("type", out var typeProp) || typeProp.ValueKind != JsonValueKind.String)
    {
        errors.Add("Missing or invalid 'type' field (must be a string)");
    }
    else
    {
        var typeVal = typeProp.GetString()!;
        if (!validTypes.Contains(typeVal))
            errors.Add($"Invalid type '{typeVal}' (must be one of: {string.Join(", ", validTypes)})");

        // Check parent consistency
        if (root.TryGetProperty("parent", out var parentProp))
        {
            bool parentIsNull = parentProp.ValueKind == JsonValueKind.Null;
            if (topLevelTypes.Contains(typeVal) && !parentIsNull)
                errors.Add($"Top-level type '{typeVal}' must have parent: null");
            if (childTypes.Contains(typeVal) && parentIsNull)
                errors.Add($"Child type '{typeVal}' must have a non-null parent");
        }
        else
        {
            errors.Add("Missing 'parent' field");
        }
    }

    if (!root.TryGetProperty("boundary", out var boundaryProp) || boundaryProp.ValueKind != JsonValueKind.Array)
    {
        errors.Add("Missing or invalid 'boundary' field (must be an array)");
        return errors;
    }

    var pointCount = boundaryProp.GetArrayLength();

    // Rule 1: at least 4 points (3 unique + closing)
    if (pointCount < 4)
    {
        errors.Add($"Boundary must have at least 4 points (has {pointCount})");
        return errors;
    }

    // Parse and validate each coordinate pair
    var points = new List<(double Lon, double Lat)>();
    for (int i = 0; i < pointCount; i++)
    {
        var pair = boundaryProp[i];
        if (pair.ValueKind != JsonValueKind.Array || pair.GetArrayLength() != 2)
        {
            errors.Add($"Point [{i}] must be [lon, lat] array of 2 numbers");
            continue;
        }

        var lon = pair[0].GetDouble();
        var lat = pair[1].GetDouble();
        points.Add((lon, lat));

        // Rule 3: check rounding to 3 decimal places
        if (Math.Round(lon, 3) != lon)
            errors.Add($"Point [{i}] longitude {lon} not rounded to 3 decimal places");
        if (Math.Round(lat, 3) != lat)
            errors.Add($"Point [{i}] latitude {lat} not rounded to 3 decimal places");

        // Coordinate range check (allow continuous longitude for antimeridian)
        if (lat < -90 || lat > 90)
            errors.Add($"Point [{i}] latitude {lat} out of range [-90, 90]");
        if (lon < -360 || lon > 360)
            errors.Add($"Point [{i}] longitude {lon} out of range [-360, 360]");
    }

    if (points.Count < 4)
        return errors;

    // Rule 2: first point must equal last point (closed ring)
    var first = points[0];
    var last = points[^1];
    if (first.Lon != last.Lon || first.Lat != last.Lat)
        errors.Add($"Ring not closed: first [{first.Lon}, {first.Lat}] != last [{last.Lon}, {last.Lat}]");

    // Rule 6: no duplicate consecutive points (other than closing point)
    for (int i = 0; i < points.Count - 1; i++)
    {
        var curr = points[i];
        var next = points[i + 1];
        if (i < points.Count - 2 && curr.Lon == next.Lon && curr.Lat == next.Lat)
            errors.Add($"Duplicate consecutive point at [{i}] and [{i + 1}]: [{curr.Lon}, {curr.Lat}]");
    }

    return errors;
}

static int RunNormalize(string[] args)
{
    var files = ResolveFiles(args);
    bool dryRun = args.Contains("--dry-run");

    if (files.Count == 0)
    {
        Console.WriteLine("No zone files found.");
        return 1;
    }

    int modified = 0;
    int errors = 0;

    foreach (var file in files)
    {
        try
        {
            var result = NormalizeFile(file, dryRun);
            if (result == NormalizeResult.Modified)
                modified++;
            else if (result == NormalizeResult.Error)
                errors++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {RelativePath(file)}: {ex.Message}");
            errors++;
        }
    }

    Console.WriteLine();
    Console.WriteLine($"Processed {files.Count} files: {modified} modified, {errors} error(s).");
    return errors > 0 ? 1 : 0;
}

static NormalizeResult NormalizeFile(string filePath, bool dryRun)
{
    var json = File.ReadAllText(filePath);
    JsonDocument doc;
    try
    {
        doc = JsonDocument.Parse(json);
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"ERROR: {RelativePath(filePath)}: Invalid JSON — {ex.Message}");
        return NormalizeResult.Error;
    }

    var root = doc.RootElement;

    if (!root.TryGetProperty("name", out var nameProp) ||
        !root.TryGetProperty("type", out var typeProp) ||
        !root.TryGetProperty("parent", out var parentProp) ||
        !root.TryGetProperty("boundary", out var boundaryProp))
    {
        Console.WriteLine($"ERROR: {RelativePath(filePath)}: Missing required fields");
        return NormalizeResult.Error;
    }

    var name = nameProp.GetString()!;
    var type = typeProp.GetString()!;
    string? parent = parentProp.ValueKind == JsonValueKind.Null ? null : parentProp.GetString();

    var points = new List<(double Lon, double Lat)>();
    for (int i = 0; i < boundaryProp.GetArrayLength(); i++)
    {
        var pair = boundaryProp[i];
        points.Add((pair[0].GetDouble(), pair[1].GetDouble()));
    }

    bool changed = false;

    // Normalize: round to 3 decimal places
    for (int i = 0; i < points.Count; i++)
    {
        var (lon, lat) = points[i];
        var roundedLon = Math.Round(lon, 3);
        var roundedLat = Math.Round(lat, 3);
        if (roundedLon != lon || roundedLat != lat)
        {
            points[i] = (roundedLon, roundedLat);
            changed = true;
        }
    }

    // Normalize: remove duplicate consecutive points (except closing)
    var dedupedPoints = new List<(double Lon, double Lat)> { points[0] };
    for (int i = 1; i < points.Count - 1; i++)
    {
        if (points[i].Lon != points[i - 1].Lon || points[i].Lat != points[i - 1].Lat)
            dedupedPoints.Add(points[i]);
        else
            changed = true;
    }
    // Keep the closing point
    if (points.Count > 1)
        dedupedPoints.Add(points[^1]);
    points = dedupedPoints;

    // Normalize: ensure ring is closed
    if (points.Count > 0)
    {
        var first = points[0];
        var last = points[^1];
        if (first.Lon != last.Lon || first.Lat != last.Lat)
        {
            points.Add(first);
            changed = true;
        }
    }

    // Rebuild JSON with consistent formatting
    var normalized = BuildJson(name, type, parent, points);

    if (normalized == json)
        return NormalizeResult.Unchanged;

    if (dryRun)
    {
        Console.WriteLine($"WOULD MODIFY: {RelativePath(filePath)}");
        if (changed)
            Console.WriteLine("  (coordinates were adjusted)");
        else
            Console.WriteLine("  (formatting only)");
        return NormalizeResult.Modified;
    }

    File.WriteAllText(filePath, normalized);
    Console.WriteLine($"MODIFIED: {RelativePath(filePath)}");
    return NormalizeResult.Modified;
}

static string BuildJson(string name, string type, string? parent, List<(double Lon, double Lat)> points)
{
    using var stream = new MemoryStream();
    using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
    {
        Indented = true,
    });

    writer.WriteStartObject();
    writer.WriteString("name", name);
    writer.WriteString("type", type);
    if (parent == null)
        writer.WriteNull("parent");
    else
        writer.WriteString("parent", parent);

    writer.WritePropertyName("boundary");
    writer.WriteStartArray();
    foreach (var (lon, lat) in points)
    {
        writer.WriteStartArray();
        WriteRoundedNumber(writer, lon);
        WriteRoundedNumber(writer, lat);
        writer.WriteEndArray();
    }
    writer.WriteEndArray();

    writer.WriteEndObject();
    writer.Flush();

    return System.Text.Encoding.UTF8.GetString(stream.ToArray()) + "\n";
}

static void WriteRoundedNumber(Utf8JsonWriter writer, double value)
{
    // Write as integer if it's a whole number, otherwise with minimal decimal places up to 3
    if (value == Math.Floor(value))
        writer.WriteNumberValue((long)value);
    else
        writer.WriteNumberValue(Math.Round(value, 3));
}

static string RelativePath(string path)
{
    var cwd = Directory.GetCurrentDirectory();
    if (path.StartsWith(cwd))
        return path[(cwd.Length + 1)..];
    return path;
}

static int RunSubtract(string[] args)
{
    var paths = args.Where(a => !a.StartsWith("--")).ToArray();
    bool dryRun = args.Contains("--dry-run");

    if (paths.Length < 2)
    {
        Console.WriteLine("ERROR: subtract requires two arguments: <subject.json> <clip.json>");
        Console.WriteLine("  The clip polygon is subtracted from the subject polygon.");
        Console.WriteLine("  The result overwrites the subject file.");
        return 1;
    }

    var subjectPath = paths[0];
    var clipPath = paths[1];

    if (!File.Exists(subjectPath))
    {
        Console.WriteLine($"ERROR: Subject file not found: {subjectPath}");
        return 1;
    }
    if (!File.Exists(clipPath))
    {
        Console.WriteLine($"ERROR: Clip file not found: {clipPath}");
        return 1;
    }

    var subject = LoadZonePoints(subjectPath);
    var clip = LoadZonePoints(clipPath);
    if (subject == null || clip == null)
        return 1;

    var result = PolygonSubtract(subject.Value.Points, clip.Value.Points);

    if (result == null || result.Count < 3)
    {
        Console.WriteLine("WARNING: Subtraction resulted in an empty or degenerate polygon.");
        Console.WriteLine("  The subject polygon is entirely contained within the clip polygon.");
        Console.WriteLine("  No changes written.");
        return 1;
    }

    // Round to 3 decimal places
    for (int i = 0; i < result.Count; i++)
        result[i] = (Math.Round(result[i].Lon, 3), Math.Round(result[i].Lat, 3));

    // Close the ring
    if (result[0] != result[^1])
        result.Add(result[0]);

    var (name, type, parent, _) = subject.Value;
    var newJson = BuildJson(name, type, parent, result);

    if (dryRun)
    {
        Console.WriteLine($"WOULD MODIFY: {RelativePath(subjectPath)}");
        Console.WriteLine($"  Before: {subject.Value.Points.Count} vertices");
        Console.WriteLine($"  After:  {result.Count} vertices");
        return 0;
    }

    File.WriteAllText(subjectPath, newJson);
    Console.WriteLine($"MODIFIED: {RelativePath(subjectPath)}");
    Console.WriteLine($"  Subtracted [{clip.Value.Name}] from [{name}]");
    Console.WriteLine($"  Before: {subject.Value.Points.Count} vertices, After: {result.Count} vertices");
    return 0;
}

static int RunSplitDifference(string[] args)
{
    var paths = args.Where(a => !a.StartsWith("--")).ToArray();
    bool dryRun = args.Contains("--dry-run");

    if (paths.Length < 2)
    {
        Console.WriteLine("ERROR: split-difference requires two arguments: <zone1.json> <zone2.json>");
        Console.WriteLine("  Finds the intersection of both polygons, computes the median line,");
        Console.WriteLine("  and adjusts both polygons so each gets half of the overlapping area.");
        return 1;
    }

    var path1 = paths[0];
    var path2 = paths[1];

    if (!File.Exists(path1))
    {
        Console.WriteLine($"ERROR: File not found: {path1}");
        return 1;
    }
    if (!File.Exists(path2))
    {
        Console.WriteLine($"ERROR: File not found: {path2}");
        return 1;
    }

    var zone1 = LoadZonePoints(path1);
    var zone2 = LoadZonePoints(path2);
    if (zone1 == null || zone2 == null)
        return 1;

    var intersection = PolygonIntersect(zone1.Value.Points, zone2.Value.Points);
    if (intersection == null || intersection.Count < 3)
    {
        Console.WriteLine("No intersection found between the two polygons. Nothing to split.");
        return 0;
    }

    var centroid1 = PolygonCentroid(zone1.Value.Points);
    var centroid2 = PolygonCentroid(zone2.Value.Points);

    // The median line is the perpendicular bisector of the segment connecting the two centroids.
    // We create a large clipping polygon on each side of this line.
    var midpoint = ((centroid1.Lon + centroid2.Lon) / 2, (centroid1.Lat + centroid2.Lat) / 2);
    double dx = centroid2.Lon - centroid1.Lon;
    double dy = centroid2.Lat - centroid1.Lat;

    // Perpendicular direction (rotated 90 degrees)
    double perpDx = -dy;
    double perpDy = dx;
    double perpLen = Math.Sqrt(perpDx * perpDx + perpDy * perpDy);
    if (perpLen < 1e-12)
    {
        Console.WriteLine("ERROR: The two zone centroids are at the same location. Cannot determine median line.");
        return 1;
    }
    perpDx /= perpLen;
    perpDy /= perpLen;

    // Normal direction (from centroid1 toward centroid2)
    double normDx = dx / perpLen;
    double normDy = dy / perpLen;

    // Build large half-plane polygons (CCW winding for Sutherland-Hodgman).
    double extent = 200.0; // degrees — more than enough to cover any polygon

    // halfPlane1: zone1's side (toward -norm direction from midpoint), CCW
    var halfPlane1 = new List<(double Lon, double Lat)>
    {
        (midpoint.Item1 + perpDx * extent - normDx * extent, midpoint.Item2 + perpDy * extent - normDy * extent),
        (midpoint.Item1 - perpDx * extent - normDx * extent, midpoint.Item2 - perpDy * extent - normDy * extent),
        (midpoint.Item1 - perpDx * extent, midpoint.Item2 - perpDy * extent),
        (midpoint.Item1 + perpDx * extent, midpoint.Item2 + perpDy * extent),
    };

    // halfPlane2: zone2's side (toward +norm direction from midpoint), CCW
    var halfPlane2 = new List<(double Lon, double Lat)>
    {
        (midpoint.Item1 - perpDx * extent + normDx * extent, midpoint.Item2 - perpDy * extent + normDy * extent),
        (midpoint.Item1 + perpDx * extent + normDx * extent, midpoint.Item2 + perpDy * extent + normDy * extent),
        (midpoint.Item1 + perpDx * extent, midpoint.Item2 + perpDy * extent),
        (midpoint.Item1 - perpDx * extent, midpoint.Item2 - perpDy * extent),
    };

    // Clip the intersection to each side of the median line.
    // halfPlane2 is on zone2's side, so its intersection portion belongs to zone2 — subtract from zone1.
    // halfPlane1 is on zone1's side, so its intersection portion belongs to zone1 — subtract from zone2.
    var partForZone1 = PolygonIntersect(intersection, halfPlane1);
    var partForZone2 = PolygonIntersect(intersection, halfPlane2);

    // Subtract from each zone the part given to the other zone.
    var result1 = zone1.Value.Points;
    var result2 = zone2.Value.Points;

    if (partForZone2 != null && partForZone2.Count >= 3)
        result1 = PolygonSubtract(result1, partForZone2) ?? result1;

    if (partForZone1 != null && partForZone1.Count >= 3)
        result2 = PolygonSubtract(result2, partForZone1) ?? result2;

    // Round and close
    result1 = RoundAndClose(result1);
    result2 = RoundAndClose(result2);

    if (result1.Count < 4 || result2.Count < 4)
    {
        Console.WriteLine("WARNING: Split resulted in a degenerate polygon. No changes written.");
        return 1;
    }

    var json1 = BuildJson(zone1.Value.Name, zone1.Value.Type, zone1.Value.Parent, result1);
    var json2 = BuildJson(zone2.Value.Name, zone2.Value.Type, zone2.Value.Parent, result2);

    if (dryRun)
    {
        Console.WriteLine($"WOULD MODIFY: {RelativePath(path1)}");
        Console.WriteLine($"  Before: {zone1.Value.Points.Count} vertices, After: {result1.Count} vertices");
        Console.WriteLine($"WOULD MODIFY: {RelativePath(path2)}");
        Console.WriteLine($"  Before: {zone2.Value.Points.Count} vertices, After: {result2.Count} vertices");
        return 0;
    }

    File.WriteAllText(path1, json1);
    File.WriteAllText(path2, json2);
    Console.WriteLine($"MODIFIED: {RelativePath(path1)}");
    Console.WriteLine($"  Split intersection with [{zone2.Value.Name}]");
    Console.WriteLine($"  Before: {zone1.Value.Points.Count} vertices, After: {result1.Count} vertices");
    Console.WriteLine($"MODIFIED: {RelativePath(path2)}");
    Console.WriteLine($"  Split intersection with [{zone1.Value.Name}]");
    Console.WriteLine($"  Before: {zone2.Value.Points.Count} vertices, After: {result2.Count} vertices");
    return 0;
}

static List<(double Lon, double Lat)> RoundAndClose(List<(double Lon, double Lat)> points)
{
    var result = new List<(double Lon, double Lat)>();
    for (int i = 0; i < points.Count; i++)
        result.Add((Math.Round(points[i].Lon, 3), Math.Round(points[i].Lat, 3)));

    // Remove the closing point if present (we'll re-add it)
    if (result.Count > 1 && result[0] == result[^1])
        result.RemoveAt(result.Count - 1);

    // Close the ring
    if (result.Count >= 3)
        result.Add(result[0]);

    return result;
}

static (double Lon, double Lat) PolygonCentroid(List<(double Lon, double Lat)> polygon)
{
    double cx = 0, cy = 0, area = 0;
    int n = polygon.Count;
    for (int i = 0; i < n; i++)
    {
        int j = (i + 1) % n;
        double cross = polygon[i].Lon * polygon[j].Lat - polygon[j].Lon * polygon[i].Lat;
        area += cross;
        cx += (polygon[i].Lon + polygon[j].Lon) * cross;
        cy += (polygon[i].Lat + polygon[j].Lat) * cross;
    }
    area /= 2;
    if (Math.Abs(area) < 1e-12)
    {
        // Degenerate — fall back to simple average
        double avgLon = polygon.Average(p => p.Lon);
        double avgLat = polygon.Average(p => p.Lat);
        return (avgLon, avgLat);
    }
    cx /= (6 * area);
    cy /= (6 * area);
    return (cx, cy);
}

// Polygon intersection using Sutherland-Hodgman clipping.
// Returns the polygon that is the intersection of subject and clip, or null if empty.
static List<(double Lon, double Lat)>? PolygonIntersect(
    List<(double Lon, double Lat)> subject,
    List<(double Lon, double Lat)> clip)
{
    var output = new List<(double Lon, double Lat)>(subject);

    for (int i = 0; i < clip.Count; i++)
    {
        if (output.Count == 0)
            return null;

        var input = new List<(double Lon, double Lat)>(output);
        output.Clear();

        var edgeStart = clip[i];
        var edgeEnd = clip[(i + 1) % clip.Count];

        for (int j = 0; j < input.Count; j++)
        {
            var current = input[j];
            var previous = input[(j + input.Count - 1) % input.Count];

            bool currentInside = IsLeftOf(edgeStart, edgeEnd, current);
            bool previousInside = IsLeftOf(edgeStart, edgeEnd, previous);

            if (currentInside)
            {
                if (!previousInside)
                {
                    var ix = LineIntersection(edgeStart, edgeEnd, previous, current);
                    if (ix != null)
                        output.Add(ix.Value);
                }
                output.Add(current);
            }
            else if (previousInside)
            {
                var ix = LineIntersection(edgeStart, edgeEnd, previous, current);
                if (ix != null)
                    output.Add(ix.Value);
            }
        }
    }

    return output.Count >= 3 ? output : null;
}

static bool IsLeftOf((double Lon, double Lat) edgeStart, (double Lon, double Lat) edgeEnd, (double Lon, double Lat) point)
{
    return (edgeEnd.Lon - edgeStart.Lon) * (point.Lat - edgeStart.Lat)
         - (edgeEnd.Lat - edgeStart.Lat) * (point.Lon - edgeStart.Lon) >= 0;
}

static (double Lon, double Lat)? LineIntersection(
    (double Lon, double Lat) a1, (double Lon, double Lat) a2,
    (double Lon, double Lat) b1, (double Lon, double Lat) b2)
{
    double x1 = a1.Lon, y1 = a1.Lat;
    double x2 = a2.Lon, y2 = a2.Lat;
    double x3 = b1.Lon, y3 = b1.Lat;
    double x4 = b2.Lon, y4 = b2.Lat;

    double denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
    if (Math.Abs(denom) < 1e-12)
        return null;

    double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denom;
    double ix = x1 + t * (x2 - x1);
    double iy = y1 + t * (y2 - y1);
    return (ix, iy);
}

static (string Name, string Type, string? Parent, List<(double Lon, double Lat)> Points)? LoadZonePoints(string path)
{
    try
    {
        var json = File.ReadAllText(path);
        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var name = root.GetProperty("name").GetString()!;
        var type = root.GetProperty("type").GetString()!;
        var parentProp = root.GetProperty("parent");
        string? parent = parentProp.ValueKind == JsonValueKind.Null ? null : parentProp.GetString();

        var boundaryProp = root.GetProperty("boundary");
        var points = new List<(double Lon, double Lat)>();
        for (int i = 0; i < boundaryProp.GetArrayLength(); i++)
        {
            var pair = boundaryProp[i];
            points.Add((pair[0].GetDouble(), pair[1].GetDouble()));
        }

        // Remove closing point for computation (we work with open rings)
        if (points.Count > 1 && points[0] == points[^1])
            points.RemoveAt(points.Count - 1);

        return (name, type, parent, points);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: Cannot load {RelativePath(path)}: {ex.Message}");
        return null;
    }
}

// Polygon subtraction: A - B using Weiler-Atherton boundary-walking approach.
// Returns the portion of A that does not overlap with B.
static List<(double Lon, double Lat)>? PolygonSubtract(
    List<(double Lon, double Lat)> subject,
    List<(double Lon, double Lat)> clip)
{
    // Step 1: Check if there's any overlap at all
    bool anySubjectInsideClip = false;
    bool anySubjectOutsideClip = false;
    foreach (var p in subject)
    {
        if (PointInPolygon(p, clip))
            anySubjectInsideClip = true;
        else
            anySubjectOutsideClip = true;
        if (anySubjectInsideClip && anySubjectOutsideClip) break;
    }

    if (!anySubjectInsideClip)
    {
        // Check if any clip edges intersect subject edges
        bool hasIntersection = false;
        for (int i = 0; i < subject.Count && !hasIntersection; i++)
        {
            var a1 = subject[i];
            var a2 = subject[(i + 1) % subject.Count];
            for (int j = 0; j < clip.Count; j++)
            {
                var b1 = clip[j];
                var b2 = clip[(j + 1) % clip.Count];
                if (SegmentIntersection(a1, a2, b1, b2) != null)
                {
                    hasIntersection = true;
                    break;
                }
            }
        }
        if (!hasIntersection)
            return new List<(double Lon, double Lat)>(subject); // No overlap
    }

    if (!anySubjectOutsideClip)
    {
        // Check if any clip edges intersect subject edges (subject might fully contain clip)
        bool hasIntersection = false;
        for (int i = 0; i < subject.Count && !hasIntersection; i++)
        {
            var a1 = subject[i];
            var a2 = subject[(i + 1) % subject.Count];
            for (int j = 0; j < clip.Count; j++)
            {
                var b1 = clip[j];
                var b2 = clip[(j + 1) % clip.Count];
                if (SegmentIntersection(a1, a2, b1, b2) != null)
                {
                    hasIntersection = true;
                    break;
                }
            }
        }
        if (!hasIntersection)
            return null; // Subject entirely inside clip
    }

    // Step 2: Build augmented subject polygon with intersection points
    var augSubject = BuildAugmentedPolygon(subject, clip);
    var augClip = BuildAugmentedPolygon(clip, subject);

    // Step 3: Walk the boundary to construct the result
    var result = WalkSubtraction(augSubject, augClip, clip);
    return result;
}

static List<(double Lon, double Lat)>? WalkSubtraction(
    List<AugPoint> augSubject,
    List<AugPoint> augClip,
    List<(double Lon, double Lat)> clipPoly)
{
    // Find a starting point on the subject that's outside the clip
    int startIdx = -1;
    for (int i = 0; i < augSubject.Count; i++)
    {
        if (!augSubject[i].IsIntersection && !PointInPolygon(augSubject[i].Point, clipPoly))
        {
            startIdx = i;
            break;
        }
    }

    if (startIdx == -1)
    {
        // All subject vertices are inside clip. Try starting from an intersection exit point.
        for (int i = 0; i < augSubject.Count; i++)
        {
            if (augSubject[i].IsIntersection && augSubject[i].IsExit)
            {
                startIdx = i;
                break;
            }
        }
        if (startIdx == -1)
            return null;
    }

    var result = new List<(double Lon, double Lat)>();
    var visited = new HashSet<int>();
    int current = startIdx;
    bool onSubject = true;
    int safety = augSubject.Count + augClip.Count + 100;

    while (safety-- > 0)
    {
        if (onSubject)
        {
            var pt = augSubject[current];
            result.Add(pt.Point);

            if (pt.IsIntersection && pt.IsEntry && !visited.Contains(current))
            {
                visited.Add(current);
                // Switch to clip boundary (walk in reverse on clip = CW direction)
                int clipIdx = FindMatchingPoint(augClip, pt.Point);
                if (clipIdx >= 0)
                {
                    onSubject = false;
                    current = clipIdx;
                    // Move backward on clip (reverse direction for subtraction)
                    current = (current - 1 + augClip.Count) % augClip.Count;
                    continue;
                }
            }

            current = (current + 1) % augSubject.Count;
            if (current == startIdx)
                break;
        }
        else
        {
            var pt = augClip[current];
            result.Add(pt.Point);

            if (pt.IsIntersection)
            {
                // Check if this intersection gets us back to subject
                int subjIdx = FindMatchingPoint(augSubject, pt.Point);
                if (subjIdx >= 0)
                {
                    onSubject = true;
                    current = (subjIdx + 1) % augSubject.Count;
                    if (current == startIdx)
                        break;
                    continue;
                }
            }

            // Walk clip in reverse
            current = (current - 1 + augClip.Count) % augClip.Count;
        }
    }

    return result.Count >= 3 ? result : null;
}

static int FindMatchingPoint(List<AugPoint> augPoly, (double Lon, double Lat) point)
{
    double bestDist = double.MaxValue;
    int bestIdx = -1;
    for (int i = 0; i < augPoly.Count; i++)
    {
        if (!augPoly[i].IsIntersection) continue;
        double dx = augPoly[i].Point.Lon - point.Lon;
        double dy = augPoly[i].Point.Lat - point.Lat;
        double dist = dx * dx + dy * dy;
        if (dist < bestDist)
        {
            bestDist = dist;
            bestIdx = i;
        }
    }
    return bestDist < 1e-8 ? bestIdx : -1;
}

static List<AugPoint> BuildAugmentedPolygon(
    List<(double Lon, double Lat)> poly,
    List<(double Lon, double Lat)> otherPoly)
{
    var result = new List<AugPoint>();

    for (int i = 0; i < poly.Count; i++)
    {
        var a1 = poly[i];
        var a2 = poly[(i + 1) % poly.Count];

        bool a1Inside = PointInPolygon(a1, otherPoly);

        result.Add(new AugPoint { Point = a1, IsIntersection = false, T = 0 });

        // Find all intersections of this edge with otherPoly edges
        var edgeIntersections = new List<(double T, (double Lon, double Lat) Point, bool Entering)>();

        for (int j = 0; j < otherPoly.Count; j++)
        {
            var b1 = otherPoly[j];
            var b2 = otherPoly[(j + 1) % otherPoly.Count];
            var ix = SegmentIntersection(a1, a2, b1, b2);
            if (ix != null)
            {
                double t = ParameterOnSegment(a1, a2, ix.Value);
                if (t > 1e-9 && t < 1.0 - 1e-9)
                {
                    // Determine if entering or exiting otherPoly
                    // Use cross product of subject edge direction with clip edge direction
                    double edgeDx = a2.Lon - a1.Lon;
                    double edgeDy = a2.Lat - a1.Lat;
                    double clipDx = b2.Lon - b1.Lon;
                    double clipDy = b2.Lat - b1.Lat;
                    double cross = edgeDx * clipDy - edgeDy * clipDx;
                    bool entering = cross > 0; // entering if crossing left-to-right
                    edgeIntersections.Add((t, ix.Value, entering));
                }
            }
        }

        edgeIntersections.Sort((a, b) => a.T.CompareTo(b.T));
        // Deduplicate very close intersections
        for (int k = 0; k < edgeIntersections.Count - 1; k++)
        {
            if (edgeIntersections[k + 1].T - edgeIntersections[k].T < 1e-8)
            {
                edgeIntersections.RemoveAt(k + 1);
                k--;
            }
        }

        // If we have intersections, alternate entry/exit based on starting state
        bool currentlyInside = a1Inside;
        for (int k = 0; k < edgeIntersections.Count; k++)
        {
            var (t, pt, _) = edgeIntersections[k];
            bool isEntry = !currentlyInside; // if outside, this intersection enters
            bool isExit = currentlyInside;   // if inside, this intersection exits
            result.Add(new AugPoint
            {
                Point = pt,
                IsIntersection = true,
                IsEntry = isEntry,
                IsExit = isExit,
                T = t
            });
            currentlyInside = !currentlyInside;
        }
    }

    return result;
}

static double ParameterOnSegment((double Lon, double Lat) a, (double Lon, double Lat) b, (double Lon, double Lat) p)
{
    double dx = b.Lon - a.Lon;
    double dy = b.Lat - a.Lat;
    if (Math.Abs(dx) > Math.Abs(dy))
        return (p.Lon - a.Lon) / dx;
    else if (Math.Abs(dy) > 1e-12)
        return (p.Lat - a.Lat) / dy;
    return 0;
}

static bool PointInPolygon((double Lon, double Lat) point, List<(double Lon, double Lat)> polygon)
{
    bool inside = false;
    int n = polygon.Count;
    for (int i = 0, j = n - 1; i < n; j = i++)
    {
        double yi = polygon[i].Lat, xi = polygon[i].Lon;
        double yj = polygon[j].Lat, xj = polygon[j].Lon;

        if (((yi > point.Lat) != (yj > point.Lat)) &&
            (point.Lon < (xj - xi) * (point.Lat - yi) / (yj - yi) + xi))
        {
            inside = !inside;
        }
    }
    return inside;
}

// Compute intersection point of segments (a1->a2) and (b1->b2), or null if they don't intersect
static (double Lon, double Lat)? SegmentIntersection(
    (double Lon, double Lat) a1, (double Lon, double Lat) a2,
    (double Lon, double Lat) b1, (double Lon, double Lat) b2)
{
    double x1 = a1.Lon, y1 = a1.Lat;
    double x2 = a2.Lon, y2 = a2.Lat;
    double x3 = b1.Lon, y3 = b1.Lat;
    double x4 = b2.Lon, y4 = b2.Lat;

    double denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
    if (Math.Abs(denom) < 1e-12)
        return null;

    double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denom;
    double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denom;

    if (t < -1e-9 || t > 1.0 + 1e-9 || u < -1e-9 || u > 1.0 + 1e-9)
        return null;

    double ix = x1 + t * (x2 - x1);
    double iy = y1 + t * (y2 - y1);
    return (ix, iy);
}

enum NormalizeResult { Unchanged, Modified, Error }

record struct AugPoint
{
    public (double Lon, double Lat) Point;
    public bool IsIntersection;
    public bool IsEntry;
    public bool IsExit;
    public double T;
}
