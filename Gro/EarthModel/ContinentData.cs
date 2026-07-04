namespace Gro.EarthModel;

internal static class ContinentData
{
    public static void Add(List<Zone> zones)
    {
        zones.Add(new Zone
        {
            Name = "North America",
            Type = ZoneType.Continent,
            ParentName = null,
            Boundary = new[]
            {
                new GeoCoord(83, -170), new GeoCoord(83, -50),
                new GeoCoord(72, -55), new GeoCoord(60, -45),
                new GeoCoord(45, -52), new GeoCoord(30, -80),
                new GeoCoord(25, -82), new GeoCoord(15, -88),
                new GeoCoord(7, -77), new GeoCoord(15, -95),
                new GeoCoord(20, -105), new GeoCoord(32, -117),
                new GeoCoord(49, -125), new GeoCoord(55, -130),
                new GeoCoord(60, -140), new GeoCoord(65, -168),
                new GeoCoord(72, -170),
            }
        });

        zones.Add(new Zone
        {
            Name = "South America",
            Type = ZoneType.Continent,
            ParentName = null,
            Boundary = new[]
            {
                new GeoCoord(13, -75), new GeoCoord(10, -62),
                new GeoCoord(7, -52), new GeoCoord(0, -50),
                new GeoCoord(-5, -35), new GeoCoord(-23, -35),
                new GeoCoord(-35, -53), new GeoCoord(-42, -63),
                new GeoCoord(-55, -68), new GeoCoord(-56, -70),
                new GeoCoord(-46, -75), new GeoCoord(-38, -74),
                new GeoCoord(-18, -70), new GeoCoord(-5, -81),
                new GeoCoord(2, -79),
            }
        });

        zones.Add(new Zone
        {
            Name = "Europe",
            Type = ZoneType.Continent,
            ParentName = null,
            Boundary = new[]
            {
                new GeoCoord(71, -25), new GeoCoord(71, 30),
                new GeoCoord(70, 40), new GeoCoord(65, 45),
                new GeoCoord(55, 60), new GeoCoord(50, 55),
                new GeoCoord(47, 40), new GeoCoord(42, 42),
                new GeoCoord(41, 30), new GeoCoord(35, 27),
                new GeoCoord(36, -6), new GeoCoord(38, -10),
                new GeoCoord(43, -9), new GeoCoord(48, -5),
                new GeoCoord(51, -10), new GeoCoord(58, -7),
                new GeoCoord(62, -7), new GeoCoord(66, -14),
            }
        });

        zones.Add(new Zone
        {
            Name = "Africa",
            Type = ZoneType.Continent,
            ParentName = null,
            Boundary = new[]
            {
                new GeoCoord(37, -10), new GeoCoord(37, 11),
                new GeoCoord(32, 32), new GeoCoord(30, 33),
                new GeoCoord(22, 37), new GeoCoord(12, 44),
                new GeoCoord(11, 51), new GeoCoord(2, 42),
                new GeoCoord(-11, 40), new GeoCoord(-26, 35),
                new GeoCoord(-35, 20), new GeoCoord(-34, 18),
                new GeoCoord(-30, 17), new GeoCoord(-17, 12),
                new GeoCoord(-6, 12), new GeoCoord(5, -4),
                new GeoCoord(5, -8), new GeoCoord(10, -15),
                new GeoCoord(15, -17), new GeoCoord(22, -17),
                new GeoCoord(28, -13), new GeoCoord(33, -7),
                new GeoCoord(36, -6),
            }
        });

        zones.Add(new Zone
        {
            Name = "Asia",
            Type = ZoneType.Continent,
            ParentName = null,
            Boundary = new[]
            {
                new GeoCoord(77, 60), new GeoCoord(77, 180),
                new GeoCoord(65, 180), new GeoCoord(55, 163),
                new GeoCoord(45, 143), new GeoCoord(35, 130),
                new GeoCoord(22, 120), new GeoCoord(10, 105),
                new GeoCoord(1, 104), new GeoCoord(-8, 110),
                new GeoCoord(6, 95), new GeoCoord(8, 77),
                new GeoCoord(20, 72), new GeoCoord(25, 62),
                new GeoCoord(27, 44), new GeoCoord(32, 35),
                new GeoCoord(42, 42), new GeoCoord(47, 40),
                new GeoCoord(50, 55), new GeoCoord(55, 60),
                new GeoCoord(65, 60), new GeoCoord(70, 60),
            }
        });

        zones.Add(new Zone
        {
            Name = "Oceania",
            Type = ZoneType.Continent,
            ParentName = null,
            Boundary = new[]
            {
                new GeoCoord(-10, 110), new GeoCoord(-10, 155),
                new GeoCoord(-5, 155), new GeoCoord(-5, 180),
                new GeoCoord(-20, 180), new GeoCoord(-30, 175),
                new GeoCoord(-47, 170), new GeoCoord(-45, 145),
                new GeoCoord(-38, 140), new GeoCoord(-32, 115),
                new GeoCoord(-20, 113), new GeoCoord(-12, 125),
            }
        });

        zones.Add(new Zone
        {
            Name = "Antarctica",
            Type = ZoneType.Continent,
            ParentName = null,
            Boundary = new[]
            {
                new GeoCoord(-60, -180), new GeoCoord(-60, -90),
                new GeoCoord(-60, 0), new GeoCoord(-60, 90),
                new GeoCoord(-60, 180), new GeoCoord(-90, 180),
                new GeoCoord(-90, 0), new GeoCoord(-90, -180),
            }
        });
    }
}
