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
            new GeoCoord(-60, 180), new GeoCoord(-70, 180),
            new GeoCoord(-70, 0), new GeoCoord(-70, -180),
        }));

        AddAtlanticZones(zones);
        AddPacificZones(zones);
        AddIndianZones(zones);
        AddArcticZones(zones);
        AddSouthernZones(zones);
    }

    private static void AddAtlanticZones(List<Zone> zones)
    {
        // Northwest Atlantic (off US/Canada east coast)
        zones.Add(MakeOceanZone("Northwest Atlantic", "Atlantic Ocean", new[]
        {
            new GeoCoord(65, -80), new GeoCoord(65, -45),
            new GeoCoord(45, -40), new GeoCoord(35, -40),
            new GeoCoord(25, -50), new GeoCoord(10, -62),
            new GeoCoord(15, -88), new GeoCoord(25, -82),
            new GeoCoord(30, -80), new GeoCoord(45, -55),
        }));

        // Northeast Atlantic (between Iceland and Europe)
        zones.Add(MakeOceanZone("Northeast Atlantic", "Atlantic Ocean", new[]
        {
            new GeoCoord(65, -45), new GeoCoord(65, -10),
            new GeoCoord(35, -6), new GeoCoord(25, -20),
            new GeoCoord(35, -40), new GeoCoord(45, -40),
        }));

        // Central Atlantic (tropical belt)
        zones.Add(MakeOceanZone("Central Atlantic", "Atlantic Ocean", new[]
        {
            new GeoCoord(25, -50), new GeoCoord(25, -20),
            new GeoCoord(10, -15), new GeoCoord(5, -8),
            new GeoCoord(0, -5), new GeoCoord(0, -25),
            new GeoCoord(10, -62),
        }));

        // Southwest Atlantic (off Brazil/Argentina)
        zones.Add(MakeOceanZone("Southwest Atlantic", "Atlantic Ocean", new[]
        {
            new GeoCoord(0, -25), new GeoCoord(0, -5),
            new GeoCoord(-5, 0), new GeoCoord(-10, -5),
            new GeoCoord(-35, -5), new GeoCoord(-60, -5),
            new GeoCoord(-60, -70), new GeoCoord(-35, -55),
            new GeoCoord(-5, -35),
        }));

        // Southeast Atlantic (off West Africa)
        zones.Add(MakeOceanZone("Southeast Atlantic", "Atlantic Ocean", new[]
        {
            new GeoCoord(0, -5), new GeoCoord(-5, 12),
            new GeoCoord(-35, 20), new GeoCoord(-60, 20),
            new GeoCoord(-60, -5), new GeoCoord(-35, -5),
            new GeoCoord(-10, -5),
        }));

        // Caribbean Sea
        zones.Add(MakeOceanZone("Caribbean Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(23, -85), new GeoCoord(20, -73),
            new GeoCoord(18, -62), new GeoCoord(12, -60),
            new GeoCoord(10, -62), new GeoCoord(8, -77),
            new GeoCoord(10, -83), new GeoCoord(15, -88),
            new GeoCoord(18, -88), new GeoCoord(21, -87),
        }));

        // Gulf of Mexico
        zones.Add(MakeOceanZone("Gulf of Mexico", "Atlantic Ocean", new[]
        {
            new GeoCoord(30, -90), new GeoCoord(30, -82),
            new GeoCoord(25, -80), new GeoCoord(21, -87),
            new GeoCoord(18, -95), new GeoCoord(21, -97),
            new GeoCoord(26, -97), new GeoCoord(29, -95),
        }));

        // Mediterranean Sea
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

        // Western Mediterranean
        zones.Add(MakeOceanZone("Western Mediterranean", "Atlantic Ocean", new[]
        {
            new GeoCoord(44, 9), new GeoCoord(43, 5),
            new GeoCoord(37, -6), new GeoCoord(35, -5),
            new GeoCoord(32, 0), new GeoCoord(31, 12),
            new GeoCoord(36, 12), new GeoCoord(38, 10),
            new GeoCoord(40, 9), new GeoCoord(42, 9),
        }));

        // Eastern Mediterranean
        zones.Add(MakeOceanZone("Eastern Mediterranean", "Atlantic Ocean", new[]
        {
            new GeoCoord(38, 20), new GeoCoord(37, 36),
            new GeoCoord(35, 36), new GeoCoord(32, 35),
            new GeoCoord(31, 25), new GeoCoord(31, 20),
            new GeoCoord(33, 20), new GeoCoord(35, 20),
        }));

        // Baltic Sea
        zones.Add(MakeOceanZone("Baltic Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(66, 20), new GeoCoord(65, 26),
            new GeoCoord(60, 28), new GeoCoord(59, 24),
            new GeoCoord(55, 18), new GeoCoord(54, 10),
            new GeoCoord(55, 10), new GeoCoord(58, 10),
            new GeoCoord(60, 18),
        }));

        // North Sea
        zones.Add(MakeOceanZone("North Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(62, -2), new GeoCoord(61, 5),
            new GeoCoord(57, 8), new GeoCoord(54, 8),
            new GeoCoord(51, 4), new GeoCoord(51, -1),
            new GeoCoord(53, 0), new GeoCoord(56, -2),
            new GeoCoord(58, -3),
        }));

        // Black Sea
        zones.Add(MakeOceanZone("Black Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(46, 28), new GeoCoord(44, 37),
            new GeoCoord(42, 41), new GeoCoord(41, 40),
            new GeoCoord(41, 28), new GeoCoord(42, 28),
            new GeoCoord(44, 28), new GeoCoord(46, 30),
        }));

        // Bay of Biscay
        zones.Add(MakeOceanZone("Bay of Biscay", "Atlantic Ocean", new[]
        {
            new GeoCoord(48, -8), new GeoCoord(48, -1),
            new GeoCoord(44, -1), new GeoCoord(43, -3),
            new GeoCoord(43, -8), new GeoCoord(44, -9),
            new GeoCoord(46, -9),
        }));

        // Sargasso Sea (central North Atlantic gyre)
        zones.Add(MakeOceanZone("Sargasso Sea", "Atlantic Ocean", new[]
        {
            new GeoCoord(35, -70), new GeoCoord(35, -40),
            new GeoCoord(25, -40), new GeoCoord(25, -50),
            new GeoCoord(25, -70), new GeoCoord(30, -75),
        }));
    }

    private static void AddPacificZones(List<Zone> zones)
    {
        // Northwest Pacific (off Japan/Kamchatka)
        zones.Add(MakeOceanZone("Northwest Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(65, -170), new GeoCoord(65, 180),
            new GeoCoord(55, 163), new GeoCoord(45, 143),
            new GeoCoord(35, 130), new GeoCoord(30, 140),
            new GeoCoord(25, 155), new GeoCoord(25, 180),
            new GeoCoord(50, 180), new GeoCoord(55, -170),
            new GeoCoord(60, -170),
        }));

        // Northeast Pacific (off US/Canada west coast)
        zones.Add(MakeOceanZone("Northeast Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(55, -170), new GeoCoord(50, 180),
            new GeoCoord(25, 180), new GeoCoord(25, -130),
            new GeoCoord(30, -120), new GeoCoord(49, -125),
            new GeoCoord(55, -130), new GeoCoord(60, -140),
            new GeoCoord(65, -168),
        }));

        // Central West Pacific (Micronesia/Melanesia region)
        zones.Add(MakeOceanZone("Central West Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(25, 130), new GeoCoord(25, 155),
            new GeoCoord(10, 160), new GeoCoord(0, 155),
            new GeoCoord(-10, 155), new GeoCoord(-10, 140),
            new GeoCoord(-8, 140), new GeoCoord(22, 120),
            new GeoCoord(25, 125),
        }));

        // Central East Pacific (tropical, off Mexico/Central America)
        zones.Add(MakeOceanZone("Central East Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(25, -130), new GeoCoord(25, 180),
            new GeoCoord(10, 180), new GeoCoord(10, -105),
            new GeoCoord(15, -95), new GeoCoord(20, -105),
            new GeoCoord(25, -115),
        }));

        // Equatorial Pacific (0-10N band across the ocean)
        zones.Add(MakeOceanZone("Equatorial Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(10, 160), new GeoCoord(10, 180),
            new GeoCoord(10, -105), new GeoCoord(0, -81),
            new GeoCoord(0, -80), new GeoCoord(-5, -81),
            new GeoCoord(0, -81), new GeoCoord(0, 155),
        }));

        // Southwest Pacific (Polynesia, off New Zealand)
        zones.Add(MakeOceanZone("Southwest Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(0, 155), new GeoCoord(0, 180),
            new GeoCoord(-30, 175), new GeoCoord(-60, 180),
            new GeoCoord(-60, -180), new GeoCoord(-60, -130),
            new GeoCoord(-30, -130), new GeoCoord(0, -130),
            new GeoCoord(0, 155),
        }));

        // Southeast Pacific (off Chile/Peru)
        zones.Add(MakeOceanZone("Southeast Pacific", "Pacific Ocean", new[]
        {
            new GeoCoord(0, -130), new GeoCoord(-30, -130),
            new GeoCoord(-60, -130), new GeoCoord(-60, -70),
            new GeoCoord(-45, -75), new GeoCoord(-5, -81),
            new GeoCoord(0, -80),
        }));

        // Sea of Japan
        zones.Add(MakeOceanZone("Sea of Japan", "Pacific Ocean", new[]
        {
            new GeoCoord(52, 132), new GeoCoord(48, 140),
            new GeoCoord(43, 141), new GeoCoord(39, 140),
            new GeoCoord(35, 130), new GeoCoord(36, 128),
            new GeoCoord(40, 129), new GeoCoord(43, 131),
            new GeoCoord(47, 135),
        }));

        // South China Sea
        zones.Add(MakeOceanZone("South China Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(22, 108), new GeoCoord(22, 120),
            new GeoCoord(15, 120), new GeoCoord(7, 117),
            new GeoCoord(3, 108), new GeoCoord(1, 104),
            new GeoCoord(10, 105), new GeoCoord(16, 108),
        }));

        // East China Sea
        zones.Add(MakeOceanZone("East China Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(33, 120), new GeoCoord(33, 128),
            new GeoCoord(30, 130), new GeoCoord(25, 125),
            new GeoCoord(22, 120), new GeoCoord(25, 120),
            new GeoCoord(28, 120), new GeoCoord(31, 121),
        }));

        // Coral Sea
        zones.Add(MakeOceanZone("Coral Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(-10, 146), new GeoCoord(-10, 165),
            new GeoCoord(-20, 165), new GeoCoord(-25, 158),
            new GeoCoord(-25, 153), new GeoCoord(-20, 148),
            new GeoCoord(-15, 145),
        }));

        // Bering Sea
        zones.Add(MakeOceanZone("Bering Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(65, 163), new GeoCoord(65, -170),
            new GeoCoord(60, -170), new GeoCoord(55, -165),
            new GeoCoord(52, -170), new GeoCoord(52, 170),
            new GeoCoord(55, 163), new GeoCoord(60, 163),
        }));

        // Sea of Okhotsk
        zones.Add(MakeOceanZone("Sea of Okhotsk", "Pacific Ocean", new[]
        {
            new GeoCoord(60, 138), new GeoCoord(60, 155),
            new GeoCoord(55, 156), new GeoCoord(50, 155),
            new GeoCoord(45, 145), new GeoCoord(43, 142),
            new GeoCoord(46, 138), new GeoCoord(52, 138),
        }));

        // Philippine Sea
        zones.Add(MakeOceanZone("Philippine Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(30, 130), new GeoCoord(30, 140),
            new GeoCoord(22, 140), new GeoCoord(15, 135),
            new GeoCoord(10, 130), new GeoCoord(15, 120),
            new GeoCoord(22, 120), new GeoCoord(25, 125),
        }));

        // Tasman Sea
        zones.Add(MakeOceanZone("Tasman Sea", "Pacific Ocean", new[]
        {
            new GeoCoord(-28, 152), new GeoCoord(-28, 170),
            new GeoCoord(-35, 170), new GeoCoord(-45, 168),
            new GeoCoord(-45, 150), new GeoCoord(-38, 148),
            new GeoCoord(-32, 150),
        }));

        // Gulf of Alaska
        zones.Add(MakeOceanZone("Gulf of Alaska", "Pacific Ocean", new[]
        {
            new GeoCoord(60, -148), new GeoCoord(60, -135),
            new GeoCoord(55, -130), new GeoCoord(50, -130),
            new GeoCoord(50, -145), new GeoCoord(55, -155),
            new GeoCoord(58, -152),
        }));
    }

    private static void AddIndianZones(List<Zone> zones)
    {
        // North Indian Ocean (between Arabia and India)
        zones.Add(MakeOceanZone("North Indian Ocean", "Indian Ocean", new[]
        {
            new GeoCoord(25, 55), new GeoCoord(20, 72),
            new GeoCoord(8, 77), new GeoCoord(6, 95),
            new GeoCoord(0, 80), new GeoCoord(0, 55),
            new GeoCoord(12, 44), new GeoCoord(25, 40),
        }));

        // Central Indian Ocean (equatorial region)
        zones.Add(MakeOceanZone("Central Indian Ocean", "Indian Ocean", new[]
        {
            new GeoCoord(0, 55), new GeoCoord(0, 80),
            new GeoCoord(0, 95), new GeoCoord(-8, 110),
            new GeoCoord(-30, 100), new GeoCoord(-30, 55),
            new GeoCoord(-10, 40), new GeoCoord(0, 42),
        }));

        // Southwest Indian Ocean (off Madagascar/East Africa)
        zones.Add(MakeOceanZone("Southwest Indian Ocean", "Indian Ocean", new[]
        {
            new GeoCoord(-10, 40), new GeoCoord(-30, 55),
            new GeoCoord(-60, 55), new GeoCoord(-60, 20),
            new GeoCoord(-35, 20), new GeoCoord(-26, 35),
            new GeoCoord(-11, 40),
        }));

        // Southeast Indian Ocean (off Western Australia)
        zones.Add(MakeOceanZone("Southeast Indian Ocean", "Indian Ocean", new[]
        {
            new GeoCoord(-30, 55), new GeoCoord(-30, 100),
            new GeoCoord(-10, 110), new GeoCoord(-32, 115),
            new GeoCoord(-38, 140), new GeoCoord(-60, 140),
            new GeoCoord(-60, 55),
        }));

        // Arabian Sea
        zones.Add(MakeOceanZone("Arabian Sea", "Indian Ocean", new[]
        {
            new GeoCoord(25, 55), new GeoCoord(25, 62),
            new GeoCoord(20, 72), new GeoCoord(15, 74),
            new GeoCoord(8, 77), new GeoCoord(8, 55),
            new GeoCoord(12, 44), new GeoCoord(15, 52),
            new GeoCoord(22, 55),
        }));

        // Bay of Bengal
        zones.Add(MakeOceanZone("Bay of Bengal", "Indian Ocean", new[]
        {
            new GeoCoord(22, 80), new GeoCoord(22, 92),
            new GeoCoord(16, 95), new GeoCoord(6, 95),
            new GeoCoord(6, 80), new GeoCoord(8, 77),
            new GeoCoord(15, 80), new GeoCoord(20, 86),
        }));

        // Red Sea
        zones.Add(MakeOceanZone("Red Sea", "Indian Ocean", new[]
        {
            new GeoCoord(30, 32), new GeoCoord(28, 34),
            new GeoCoord(22, 37), new GeoCoord(18, 40),
            new GeoCoord(13, 43), new GeoCoord(12, 44),
            new GeoCoord(12, 42), new GeoCoord(14, 41),
            new GeoCoord(20, 38), new GeoCoord(24, 36),
            new GeoCoord(28, 33), new GeoCoord(30, 33),
        }));

        // Persian Gulf
        zones.Add(MakeOceanZone("Persian Gulf", "Indian Ocean", new[]
        {
            new GeoCoord(30, 48), new GeoCoord(29, 50),
            new GeoCoord(27, 52), new GeoCoord(25, 54),
            new GeoCoord(24, 52), new GeoCoord(26, 50),
            new GeoCoord(28, 48), new GeoCoord(30, 47),
        }));

        // Mozambique Channel
        zones.Add(MakeOceanZone("Mozambique Channel", "Indian Ocean", new[]
        {
            new GeoCoord(-12, 40), new GeoCoord(-12, 44),
            new GeoCoord(-16, 46), new GeoCoord(-22, 44),
            new GeoCoord(-26, 42), new GeoCoord(-26, 35),
            new GeoCoord(-20, 35), new GeoCoord(-15, 38),
        }));

        // Andaman Sea
        zones.Add(MakeOceanZone("Andaman Sea", "Indian Ocean", new[]
        {
            new GeoCoord(16, 92), new GeoCoord(16, 98),
            new GeoCoord(10, 98), new GeoCoord(6, 95),
            new GeoCoord(6, 92), new GeoCoord(10, 92),
            new GeoCoord(14, 93),
        }));
    }

    private static void AddArcticZones(List<Zone> zones)
    {
        // Central Arctic
        zones.Add(MakeOceanZone("Central Arctic", "Arctic Ocean", new[]
        {
            new GeoCoord(90, -180), new GeoCoord(90, 0), new GeoCoord(90, 180),
            new GeoCoord(80, 180), new GeoCoord(80, 90),
            new GeoCoord(80, 0), new GeoCoord(80, -90),
            new GeoCoord(80, -180),
        }));

        // Barents Sea
        zones.Add(MakeOceanZone("Barents Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(80, 20), new GeoCoord(80, 55),
            new GeoCoord(72, 55), new GeoCoord(68, 40),
            new GeoCoord(70, 30), new GeoCoord(72, 20),
        }));

        // Norwegian Sea
        zones.Add(MakeOceanZone("Norwegian Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(72, -10), new GeoCoord(72, 20),
            new GeoCoord(65, 15), new GeoCoord(62, 5),
            new GeoCoord(62, -5), new GeoCoord(65, -10),
            new GeoCoord(68, -10),
        }));

        // Beaufort Sea
        zones.Add(MakeOceanZone("Beaufort Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(76, -155), new GeoCoord(76, -125),
            new GeoCoord(72, -125), new GeoCoord(69, -140),
            new GeoCoord(70, -145), new GeoCoord(71, -155),
        }));

        // Kara Sea
        zones.Add(MakeOceanZone("Kara Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(80, 55), new GeoCoord(80, 100),
            new GeoCoord(75, 100), new GeoCoord(72, 80),
            new GeoCoord(70, 60), new GeoCoord(72, 55),
        }));

        // Laptev Sea
        zones.Add(MakeOceanZone("Laptev Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(80, 100), new GeoCoord(80, 140),
            new GeoCoord(75, 140), new GeoCoord(72, 130),
            new GeoCoord(72, 105), new GeoCoord(75, 100),
        }));

        // East Siberian Sea
        zones.Add(MakeOceanZone("East Siberian Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(80, 140), new GeoCoord(80, 180),
            new GeoCoord(72, 180), new GeoCoord(70, 160),
            new GeoCoord(72, 140), new GeoCoord(75, 140),
        }));

        // Greenland Sea
        zones.Add(MakeOceanZone("Greenland Sea", "Arctic Ocean", new[]
        {
            new GeoCoord(80, -10), new GeoCoord(80, 20),
            new GeoCoord(72, 20), new GeoCoord(72, -10),
            new GeoCoord(75, -20), new GeoCoord(80, -20),
        }));

        // Canadian Arctic Archipelago
        zones.Add(MakeOceanZone("Canadian Arctic", "Arctic Ocean", new[]
        {
            new GeoCoord(83, -60), new GeoCoord(80, -60),
            new GeoCoord(75, -80), new GeoCoord(72, -100),
            new GeoCoord(72, -130), new GeoCoord(76, -155),
            new GeoCoord(80, -155), new GeoCoord(83, -170),
            new GeoCoord(80, -180), new GeoCoord(80, -90),
        }));
    }

    private static void AddSouthernZones(List<Zone> zones)
    {
        // South Atlantic sector (20W to 70E)
        zones.Add(MakeOceanZone("Southern Atlantic Sector", "Southern Ocean", new[]
        {
            new GeoCoord(-60, -70), new GeoCoord(-60, -10),
            new GeoCoord(-70, -10), new GeoCoord(-70, -70),
        }));

        // South African sector
        zones.Add(MakeOceanZone("Southern African Sector", "Southern Ocean", new[]
        {
            new GeoCoord(-60, -10), new GeoCoord(-60, 60),
            new GeoCoord(-70, 60), new GeoCoord(-70, -10),
        }));

        // South Indian sector
        zones.Add(MakeOceanZone("Southern Indian Sector", "Southern Ocean", new[]
        {
            new GeoCoord(-60, 60), new GeoCoord(-60, 140),
            new GeoCoord(-70, 140), new GeoCoord(-70, 60),
        }));

        // South Pacific sector (Australian side)
        zones.Add(MakeOceanZone("Southern Pacific Sector (West)", "Southern Ocean", new[]
        {
            new GeoCoord(-60, 140), new GeoCoord(-60, 180),
            new GeoCoord(-70, 180), new GeoCoord(-70, 140),
        }));

        // South Pacific sector (American side)
        zones.Add(MakeOceanZone("Southern Pacific Sector (East)", "Southern Ocean", new[]
        {
            new GeoCoord(-60, -180), new GeoCoord(-60, -70),
            new GeoCoord(-70, -70), new GeoCoord(-70, -180),
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
