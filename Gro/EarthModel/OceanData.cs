namespace Gro.EarthModel;

internal static class OceanData
{
    public static void Add(List<Zone> zones)
    {
        // Ocean basins (top-level)
        zones.Add(MakeBasin("Atlantic Ocean", new[]
        {
            new GeoCoord(65, -80), new GeoCoord(65, -10),
            new GeoCoord(35, -6), new GeoCoord(10, -15),
            new GeoCoord(5, -8), new GeoCoord(-5, 12),
            new GeoCoord(-35, 20), new GeoCoord(-60, 20),
            new GeoCoord(-60, -70), new GeoCoord(-35, -55),
            new GeoCoord(-5, -35), new GeoCoord(10, -62),
            new GeoCoord(15, -88), new GeoCoord(25, -82),
            new GeoCoord(30, -80), new GeoCoord(45, -55),
        }));

        zones.Add(MakeBasin("Pacific Ocean", new[]
        {
            new GeoCoord(65, -170), new GeoCoord(65, 180),
            new GeoCoord(55, 163), new GeoCoord(45, 143),
            new GeoCoord(35, 130), new GeoCoord(22, 120),
            new GeoCoord(-8, 140), new GeoCoord(-10, 155),
            new GeoCoord(-30, 175), new GeoCoord(-60, 180),
            new GeoCoord(-60, -70), new GeoCoord(-45, -75),
            new GeoCoord(-5, -81), new GeoCoord(15, -95),
            new GeoCoord(20, -105), new GeoCoord(32, -117),
            new GeoCoord(49, -125), new GeoCoord(55, -130),
            new GeoCoord(60, -140), new GeoCoord(65, -168),
        }));

        zones.Add(MakeBasin("Indian Ocean", new[]
        {
            new GeoCoord(25, 40), new GeoCoord(25, 60),
            new GeoCoord(20, 72), new GeoCoord(8, 77),
            new GeoCoord(6, 95), new GeoCoord(-8, 110),
            new GeoCoord(-10, 110), new GeoCoord(-32, 115),
            new GeoCoord(-38, 140), new GeoCoord(-60, 140),
            new GeoCoord(-60, 20), new GeoCoord(-35, 20),
            new GeoCoord(-26, 35), new GeoCoord(-11, 40),
            new GeoCoord(2, 42), new GeoCoord(12, 44),
        }));

        zones.Add(MakeBasin("Arctic Ocean", new[]
        {
            new GeoCoord(90, -180), new GeoCoord(90, 0), new GeoCoord(90, 180),
            new GeoCoord(72, 180), new GeoCoord(77, 60),
            new GeoCoord(70, 40), new GeoCoord(71, 30),
            new GeoCoord(71, -25), new GeoCoord(83, -60),
            new GeoCoord(83, -170), new GeoCoord(72, -170),
        }));

        zones.Add(MakeBasin("Southern Ocean", new[]
        {
            new GeoCoord(-60, -180), new GeoCoord(-60, -90),
            new GeoCoord(-60, 0), new GeoCoord(-60, 90),
            new GeoCoord(-60, 180), new GeoCoord(-90, 180),
            new GeoCoord(-90, 0), new GeoCoord(-90, -180),
        }));

        // Atlantic Ocean zones
        zones.Add(MakeOceanZone("North Atlantic", "Atlantic Ocean", new[]
        {
            new GeoCoord(65, -80), new GeoCoord(65, -10),
            new GeoCoord(35, -6), new GeoCoord(25, -20),
            new GeoCoord(10, -25), new GeoCoord(10, -62),
            new GeoCoord(15, -88), new GeoCoord(25, -82),
            new GeoCoord(30, -80), new GeoCoord(45, -55),
        }));

        zones.Add(MakeOceanZone("South Atlantic", "Atlantic Ocean", new[]
        {
            new GeoCoord(10, -25), new GeoCoord(5, -8),
            new GeoCoord(-5, 12), new GeoCoord(-35, 20),
            new GeoCoord(-60, 20), new GeoCoord(-60, -70),
            new GeoCoord(-35, -55), new GeoCoord(-5, -35),
            new GeoCoord(10, -62),
        }));

        zones.Add(MakeOceanZone("Caribbean Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(23, -85), new GeoCoord(20, -73),
            new GeoCoord(18, -62), new GeoCoord(12, -60),
            new GeoCoord(10, -62), new GeoCoord(8, -77),
            new GeoCoord(10, -83), new GeoCoord(15, -88),
            new GeoCoord(18, -88), new GeoCoord(21, -87),
        }));

        zones.Add(MakeOceanZone("Gulf of Mexico", "Atlantic Ocean", new[]
        {
            new GeoCoord(30, -90), new GeoCoord(30, -82),
            new GeoCoord(25, -80), new GeoCoord(21, -87),
            new GeoCoord(18, -95), new GeoCoord(21, -97),
            new GeoCoord(26, -97), new GeoCoord(29, -95),
        }));

        zones.Add(MakeOceanZone("Mediterranean Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(46, 6), new GeoCoord(43, 5),
            new GeoCoord(37, -6), new GeoCoord(35, -5),
            new GeoCoord(32, 0), new GeoCoord(31, 12),
            new GeoCoord(31, 25), new GeoCoord(32, 32),
            new GeoCoord(35, 36), new GeoCoord(37, 36),
            new GeoCoord(41, 30), new GeoCoord(42, 20),
            new GeoCoord(45, 14), new GeoCoord(44, 9),
        }));

        zones.Add(MakeOceanZone("Western Mediterranean", "Atlantic Ocean", new[]
        {
            new GeoCoord(44, 9), new GeoCoord(43, 5),
            new GeoCoord(37, -6), new GeoCoord(35, -5),
            new GeoCoord(32, 0), new GeoCoord(31, 12),
            new GeoCoord(36, 12), new GeoCoord(38, 10),
            new GeoCoord(40, 9), new GeoCoord(42, 9),
        }));

        zones.Add(MakeOceanZone("Eastern Mediterranean", "Atlantic Ocean", new[]
        {
            new GeoCoord(38, 20), new GeoCoord(37, 36),
            new GeoCoord(35, 36), new GeoCoord(32, 35),
            new GeoCoord(31, 25), new GeoCoord(31, 20),
            new GeoCoord(33, 20), new GeoCoord(35, 20),
        }));

        zones.Add(MakeOceanZone("Baltic Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(66, 20), new GeoCoord(65, 26),
            new GeoCoord(60, 28), new GeoCoord(59, 24),
            new GeoCoord(55, 18), new GeoCoord(54, 10),
            new GeoCoord(55, 10), new GeoCoord(58, 10),
            new GeoCoord(60, 18),
        }));

        zones.Add(MakeOceanZone("North Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(62, -2), new GeoCoord(61, 5),
            new GeoCoord(57, 8), new GeoCoord(54, 8),
            new GeoCoord(51, 4), new GeoCoord(51, -1),
            new GeoCoord(53, 0), new GeoCoord(56, -2),
            new GeoCoord(58, -3),
        }));

        zones.Add(MakeOceanZone("Black Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(46, 28), new GeoCoord(44, 37),
            new GeoCoord(42, 41), new GeoCoord(41, 40),
            new GeoCoord(41, 28), new GeoCoord(42, 28),
            new GeoCoord(44, 28), new GeoCoord(46, 30),
        }));

        // Pacific Ocean zones
        zones.Add(MakeOceanZone("North Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(65, -170), new GeoCoord(65, 180),
            new GeoCoord(55, 163), new GeoCoord(45, 143),
            new GeoCoord(35, 130), new GeoCoord(30, 140),
            new GeoCoord(25, 170), new GeoCoord(25, -130),
            new GeoCoord(30, -120), new GeoCoord(49, -125),
            new GeoCoord(55, -130), new GeoCoord(60, -140),
            new GeoCoord(65, -168),
        }));

        zones.Add(MakeOceanZone("South Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(0, 155), new GeoCoord(0, -80),
            new GeoCoord(-20, -75), new GeoCoord(-45, -75),
            new GeoCoord(-60, -70), new GeoCoord(-60, 180),
            new GeoCoord(-30, 175), new GeoCoord(-10, 155),
        }));

        zones.Add(MakeOceanZone("Sea of Japan", "Pacific Ocean", new[]
        {
            new GeoCoord(52, 132), new GeoCoord(48, 140),
            new GeoCoord(43, 141), new GeoCoord(39, 140),
            new GeoCoord(35, 130), new GeoCoord(36, 128),
            new GeoCoord(40, 129), new GeoCoord(43, 131),
            new GeoCoord(47, 135),
        }));

        zones.Add(MakeOceanZone("South China Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(22, 108), new GeoCoord(22, 120),
            new GeoCoord(15, 120), new GeoCoord(7, 117),
            new GeoCoord(3, 108), new GeoCoord(1, 104),
            new GeoCoord(10, 105), new GeoCoord(16, 108),
        }));

        zones.Add(MakeOceanZone("East China Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(33, 120), new GeoCoord(33, 128),
            new GeoCoord(30, 130), new GeoCoord(25, 125),
            new GeoCoord(22, 120), new GeoCoord(25, 120),
            new GeoCoord(28, 120), new GeoCoord(31, 121),
        }));

        zones.Add(MakeOceanZone("Coral Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(-10, 146), new GeoCoord(-10, 165),
            new GeoCoord(-20, 165), new GeoCoord(-25, 158),
            new GeoCoord(-25, 153), new GeoCoord(-20, 148),
            new GeoCoord(-15, 145),
        }));

        zones.Add(MakeOceanZone("Bering Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(65, 163), new GeoCoord(65, -170),
            new GeoCoord(60, -170), new GeoCoord(55, -165),
            new GeoCoord(52, -170), new GeoCoord(52, 170),
            new GeoCoord(55, 163), new GeoCoord(60, 163),
        }));

        // Indian Ocean zones
        zones.Add(MakeOceanZone("Arabian Sea", "Indian Ocean", new[]
        {
            new GeoCoord(25, 55), new GeoCoord(25, 62),
            new GeoCoord(20, 72), new GeoCoord(15, 74),
            new GeoCoord(8, 77), new GeoCoord(8, 55),
            new GeoCoord(12, 44), new GeoCoord(15, 52),
            new GeoCoord(22, 55),
        }));

        zones.Add(MakeOceanZone("Bay of Bengal", "Indian Ocean", new[]
        {
            new GeoCoord(22, 80), new GeoCoord(22, 92),
            new GeoCoord(16, 95), new GeoCoord(6, 95),
            new GeoCoord(6, 80), new GeoCoord(8, 77),
            new GeoCoord(15, 80), new GeoCoord(20, 86),
        }));

        zones.Add(MakeOceanZone("Red Sea", "Indian Ocean", new[]
        {
            new GeoCoord(30, 32), new GeoCoord(28, 34),
            new GeoCoord(22, 37), new GeoCoord(18, 40),
            new GeoCoord(13, 43), new GeoCoord(12, 44),
            new GeoCoord(12, 42), new GeoCoord(14, 41),
            new GeoCoord(20, 38), new GeoCoord(24, 36),
            new GeoCoord(28, 33), new GeoCoord(30, 33),
        }));

        zones.Add(MakeOceanZone("Persian Gulf", "Indian Ocean", new[]
        {
            new GeoCoord(30, 48), new GeoCoord(29, 50),
            new GeoCoord(27, 52), new GeoCoord(25, 54),
            new GeoCoord(24, 52), new GeoCoord(26, 50),
            new GeoCoord(28, 48), new GeoCoord(30, 47),
        }));

        zones.Add(MakeOceanZone("Mozambique Channel", "Indian Ocean", new[]
        {
            new GeoCoord(-12, 40), new GeoCoord(-12, 44),
            new GeoCoord(-16, 46), new GeoCoord(-22, 44),
            new GeoCoord(-26, 42), new GeoCoord(-26, 35),
            new GeoCoord(-20, 35), new GeoCoord(-15, 38),
        }));

        // Arctic zones
        zones.Add(MakeOceanZone("Barents Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(80, 20), new GeoCoord(80, 55),
            new GeoCoord(72, 55), new GeoCoord(68, 40),
            new GeoCoord(70, 30), new GeoCoord(72, 20),
        }));

        zones.Add(MakeOceanZone("Norwegian Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(72, -10), new GeoCoord(72, 20),
            new GeoCoord(65, 15), new GeoCoord(62, 5),
            new GeoCoord(62, -5), new GeoCoord(65, -10),
            new GeoCoord(68, -10),
        }));

        zones.Add(MakeOceanZone("Beaufort Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(76, -155), new GeoCoord(76, -125),
            new GeoCoord(72, -125), new GeoCoord(69, -140),
            new GeoCoord(70, -145), new GeoCoord(71, -155),
        }));
    }

    private static Zone MakeBasin(string name, GeoCoord[] boundary) => new()
    {
        Name = name,
        Type = ZoneType.OceanBasin,
        ParentName = null,
        Boundary = boundary,
    };

    private static Zone MakeOceanZone(string name, string parent, GeoCoord[] boundary) => new()
    {
        Name = name,
        Type = ZoneType.OceanZone,
        ParentName = parent,
        Boundary = boundary,
    };
}
