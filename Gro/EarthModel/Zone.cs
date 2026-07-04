namespace Gro.EarthModel;

public sealed class Zone
{
    public required string Name { get; init; }
    public required ZoneType Type { get; init; }
    public required string? ParentName { get; init; }
    public required GeoCoord[] Boundary { get; init; }

    public GeoCoord Centroid
    {
        get
        {
            if (Boundary.Length == 0)
                return new GeoCoord(0, 0);

            double x = 0, y = 0, z = 0;
            foreach (var p in Boundary)
            {
                double latR = p.Lat * Math.PI / 180.0;
                double lonR = p.Lon * Math.PI / 180.0;
                x += Math.Cos(latR) * Math.Cos(lonR);
                y += Math.Cos(latR) * Math.Sin(lonR);
                z += Math.Sin(latR);
            }
            x /= Boundary.Length;
            y /= Boundary.Length;
            z /= Boundary.Length;
            double lon = Math.Atan2(y, x) * 180.0 / Math.PI;
            double hyp = Math.Sqrt(x * x + y * y);
            double lat = Math.Atan2(z, hyp) * 180.0 / Math.PI;
            return new GeoCoord(lat, lon);
        }
    }

    public bool Contains(GeoCoord point)
    {
        // Ray-casting algorithm for point-in-polygon on lat/lon
        int n = Boundary.Length;
        bool inside = false;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            double yi = Boundary[i].Lat, xi = Boundary[i].Lon;
            double yj = Boundary[j].Lat, xj = Boundary[j].Lon;

            if (((yi > point.Lat) != (yj > point.Lat)) &&
                (point.Lon < (xj - xi) * (point.Lat - yi) / (yj - yi) + xi))
            {
                inside = !inside;
            }
        }
        return inside;
    }
}
