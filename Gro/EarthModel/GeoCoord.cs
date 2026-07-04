namespace Gro.EarthModel;

public readonly record struct GeoCoord(double Lat, double Lon)
{
    public const double EarthRadiusKm = 6371.0;

    public double DistanceKm(GeoCoord other)
    {
        double lat1 = Lat * Math.PI / 180.0;
        double lat2 = other.Lat * Math.PI / 180.0;
        double dLat = (other.Lat - Lat) * Math.PI / 180.0;
        double dLon = (other.Lon - Lon) * Math.PI / 180.0;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c;
    }
}
