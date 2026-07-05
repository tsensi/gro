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
    _ => PrintUsage()
};

static int PrintUsage()
{
    Console.WriteLine("Gro.ZoneTool — validate and normalize zone JSON files");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project Gro.ZoneTool -- validate [path|--all]");
    Console.WriteLine("  dotnet run --project Gro.ZoneTool -- normalize [path|--all] [--dry-run]");
    Console.WriteLine();
    Console.WriteLine("Commands:");
    Console.WriteLine("  validate    Check zone files against the schema and validation rules");
    Console.WriteLine("  normalize   Rewrite zone files with correct rounding, closure, and formatting");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  path        Path to a specific .json file or directory");
    Console.WriteLine("  --all       Process all zone files (default if no path given)");
    Console.WriteLine("  --dry-run   Show what normalize would change without writing");
    return 1;
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

enum NormalizeResult { Unchanged, Modified, Error }
