namespace Gro.EarthModel;

internal static class CountryData
{
    public static void Add(List<Zone> zones)
    {
        // North America
        zones.Add(MakeCountry("Canada", "North America", new[]
        {
            new GeoCoord(83, -141), new GeoCoord(83, -60),
            new GeoCoord(72, -55), new GeoCoord(60, -52),
            new GeoCoord(47, -53), new GeoCoord(45, -67),
            new GeoCoord(42, -83), new GeoCoord(49, -95),
            new GeoCoord(49, -123), new GeoCoord(54, -130),
            new GeoCoord(60, -139), new GeoCoord(69, -141),
        }));

        zones.Add(MakeCountry("United States", "North America", new[]
        {
            new GeoCoord(49, -123), new GeoCoord(49, -95),
            new GeoCoord(42, -83), new GeoCoord(41, -72),
            new GeoCoord(35, -75), new GeoCoord(30, -81),
            new GeoCoord(25, -80), new GeoCoord(29, -90),
            new GeoCoord(29, -95), new GeoCoord(26, -97),
            new GeoCoord(32, -106), new GeoCoord(32, -117),
            new GeoCoord(38, -122), new GeoCoord(46, -124),
        }));

        zones.Add(MakeCountry("Mexico", "North America", new[]
        {
            new GeoCoord(32, -117), new GeoCoord(32, -106),
            new GeoCoord(26, -97), new GeoCoord(21, -97),
            new GeoCoord(18, -95), new GeoCoord(15, -92),
            new GeoCoord(16, -90), new GeoCoord(21, -87),
            new GeoCoord(21, -90), new GeoCoord(20, -105),
            new GeoCoord(23, -110), new GeoCoord(28, -112),
            new GeoCoord(31, -113), new GeoCoord(32, -115),
        }));

        // South America
        zones.Add(MakeCountry("Brazil", "South America", new[]
        {
            new GeoCoord(5, -60), new GeoCoord(2, -50),
            new GeoCoord(-2, -44), new GeoCoord(-7, -35),
            new GeoCoord(-15, -39), new GeoCoord(-23, -41),
            new GeoCoord(-29, -49), new GeoCoord(-33, -53),
            new GeoCoord(-28, -58), new GeoCoord(-16, -58),
            new GeoCoord(-10, -66), new GeoCoord(-5, -70),
            new GeoCoord(0, -67), new GeoCoord(2, -60),
        }));

        zones.Add(MakeCountry("Argentina", "South America", new[]
        {
            new GeoCoord(-22, -63), new GeoCoord(-22, -58),
            new GeoCoord(-28, -56), new GeoCoord(-34, -54),
            new GeoCoord(-38, -57), new GeoCoord(-42, -63),
            new GeoCoord(-50, -69), new GeoCoord(-55, -68),
            new GeoCoord(-52, -72), new GeoCoord(-40, -72),
            new GeoCoord(-32, -70), new GeoCoord(-27, -69),
        }));

        zones.Add(MakeCountry("Colombia", "South America", new[]
        {
            new GeoCoord(12, -72), new GeoCoord(11, -75),
            new GeoCoord(7, -77), new GeoCoord(2, -79),
            new GeoCoord(-4, -70), new GeoCoord(-1, -67),
            new GeoCoord(2, -67), new GeoCoord(6, -67),
            new GeoCoord(7, -72), new GeoCoord(10, -72),
        }));

        // Europe
        zones.Add(MakeCountry("Russia (European)", "Europe", new[]
        {
            new GeoCoord(70, 30), new GeoCoord(70, 55),
            new GeoCoord(65, 55), new GeoCoord(55, 60),
            new GeoCoord(50, 55), new GeoCoord(47, 40),
            new GeoCoord(46, 30), new GeoCoord(50, 28),
            new GeoCoord(55, 28), new GeoCoord(60, 30),
            new GeoCoord(65, 32), new GeoCoord(68, 33),
        }));

        zones.Add(MakeCountry("France", "Europe", new[]
        {
            new GeoCoord(51, 2), new GeoCoord(49, 8),
            new GeoCoord(47, 8), new GeoCoord(44, 7),
            new GeoCoord(43, 3), new GeoCoord(42, -2),
            new GeoCoord(43, -2), new GeoCoord(46, -2),
            new GeoCoord(48, -5), new GeoCoord(49, -2),
        }));

        zones.Add(MakeCountry("Germany", "Europe", new[]
        {
            new GeoCoord(55, 8), new GeoCoord(54, 14),
            new GeoCoord(51, 15), new GeoCoord(49, 13),
            new GeoCoord(47, 13), new GeoCoord(47, 7),
            new GeoCoord(49, 6), new GeoCoord(51, 6),
            new GeoCoord(52, 7), new GeoCoord(54, 9),
        }));

        zones.Add(MakeCountry("United Kingdom", "Europe", new[]
        {
            new GeoCoord(58, -5), new GeoCoord(57, -2),
            new GeoCoord(53, 0), new GeoCoord(51, 1),
            new GeoCoord(50, -1), new GeoCoord(50, -5),
            new GeoCoord(52, -5), new GeoCoord(54, -3),
            new GeoCoord(55, -5), new GeoCoord(57, -6),
        }));

        zones.Add(MakeCountry("Spain", "Europe", new[]
        {
            new GeoCoord(43, -9), new GeoCoord(43, -2),
            new GeoCoord(42, 3), new GeoCoord(40, 4),
            new GeoCoord(38, 0), new GeoCoord(36, -5),
            new GeoCoord(37, -7), new GeoCoord(38, -9),
            new GeoCoord(40, -9), new GeoCoord(42, -9),
        }));

        zones.Add(MakeCountry("Italy", "Europe", new[]
        {
            new GeoCoord(47, 7), new GeoCoord(46, 13),
            new GeoCoord(44, 14), new GeoCoord(42, 14),
            new GeoCoord(40, 16), new GeoCoord(38, 16),
            new GeoCoord(37, 15), new GeoCoord(39, 13),
            new GeoCoord(40, 9), new GeoCoord(42, 10),
            new GeoCoord(44, 8), new GeoCoord(45, 7),
        }));

        zones.Add(MakeCountry("Poland", "Europe", new[]
        {
            new GeoCoord(54, 14), new GeoCoord(54, 19),
            new GeoCoord(54, 24), new GeoCoord(52, 24),
            new GeoCoord(50, 24), new GeoCoord(49, 23),
            new GeoCoord(49, 19), new GeoCoord(50, 14),
            new GeoCoord(51, 15), new GeoCoord(52, 14),
        }));

        zones.Add(MakeCountry("Ukraine", "Europe", new[]
        {
            new GeoCoord(52, 24), new GeoCoord(52, 33),
            new GeoCoord(51, 38), new GeoCoord(48, 40),
            new GeoCoord(46, 38), new GeoCoord(45, 34),
            new GeoCoord(46, 30), new GeoCoord(48, 24),
            new GeoCoord(50, 24),
        }));

        zones.Add(MakeCountry("Sweden", "Europe", new[]
        {
            new GeoCoord(69, 18), new GeoCoord(68, 22),
            new GeoCoord(64, 22), new GeoCoord(60, 19),
            new GeoCoord(56, 16), new GeoCoord(55, 13),
            new GeoCoord(57, 11), new GeoCoord(59, 11),
            new GeoCoord(63, 14), new GeoCoord(66, 15),
        }));

        zones.Add(MakeCountry("Norway", "Europe", new[]
        {
            new GeoCoord(71, 25), new GeoCoord(70, 30),
            new GeoCoord(68, 16), new GeoCoord(64, 11),
            new GeoCoord(62, 5), new GeoCoord(58, 6),
            new GeoCoord(58, 8), new GeoCoord(60, 5),
            new GeoCoord(63, 5), new GeoCoord(67, 14),
            new GeoCoord(69, 18),
        }));

        // Africa
        zones.Add(MakeCountry("Egypt", "Africa", new[]
        {
            new GeoCoord(31, 25), new GeoCoord(31, 34),
            new GeoCoord(29, 35), new GeoCoord(22, 37),
            new GeoCoord(22, 25), new GeoCoord(25, 25),
            new GeoCoord(29, 25),
        }));

        zones.Add(MakeCountry("Nigeria", "Africa", new[]
        {
            new GeoCoord(14, 3), new GeoCoord(13, 14),
            new GeoCoord(10, 14), new GeoCoord(7, 12),
            new GeoCoord(4, 8), new GeoCoord(4, 3),
            new GeoCoord(7, 3), new GeoCoord(10, 3),
        }));

        zones.Add(MakeCountry("South Africa", "Africa", new[]
        {
            new GeoCoord(-22, 17), new GeoCoord(-22, 31),
            new GeoCoord(-27, 32), new GeoCoord(-30, 31),
            new GeoCoord(-34, 26), new GeoCoord(-34, 18),
            new GeoCoord(-31, 17), new GeoCoord(-28, 17),
        }));

        zones.Add(MakeCountry("DR Congo", "Africa", new[]
        {
            new GeoCoord(5, 18), new GeoCoord(4, 30),
            new GeoCoord(2, 31), new GeoCoord(-1, 29),
            new GeoCoord(-5, 27), new GeoCoord(-8, 26),
            new GeoCoord(-11, 24), new GeoCoord(-11, 22),
            new GeoCoord(-7, 17), new GeoCoord(-4, 12),
            new GeoCoord(0, 16), new GeoCoord(3, 18),
        }));

        zones.Add(MakeCountry("Algeria", "Africa", new[]
        {
            new GeoCoord(37, 0), new GeoCoord(37, 9),
            new GeoCoord(34, 9), new GeoCoord(30, 9),
            new GeoCoord(24, 9), new GeoCoord(19, 6),
            new GeoCoord(19, -1), new GeoCoord(22, -2),
            new GeoCoord(27, -9), new GeoCoord(32, -2),
            new GeoCoord(35, -2),
        }));

        zones.Add(MakeCountry("Libya", "Africa", new[]
        {
            new GeoCoord(33, 11), new GeoCoord(32, 25),
            new GeoCoord(30, 25), new GeoCoord(22, 24),
            new GeoCoord(20, 15), new GeoCoord(23, 11),
            new GeoCoord(30, 10),
        }));

        zones.Add(MakeCountry("Sudan", "Africa", new[]
        {
            new GeoCoord(22, 24), new GeoCoord(22, 37),
            new GeoCoord(18, 38), new GeoCoord(14, 36),
            new GeoCoord(10, 34), new GeoCoord(4, 32),
            new GeoCoord(4, 24), new GeoCoord(10, 24),
            new GeoCoord(15, 24),
        }));

        zones.Add(MakeCountry("Ethiopia", "Africa", new[]
        {
            new GeoCoord(15, 36), new GeoCoord(14, 42),
            new GeoCoord(11, 43), new GeoCoord(8, 47),
            new GeoCoord(4, 42), new GeoCoord(4, 36),
            new GeoCoord(6, 35), new GeoCoord(9, 34),
            new GeoCoord(12, 36),
        }));

        zones.Add(MakeCountry("Tanzania", "Africa", new[]
        {
            new GeoCoord(-1, 30), new GeoCoord(-1, 37),
            new GeoCoord(-5, 40), new GeoCoord(-10, 40),
            new GeoCoord(-11, 35), new GeoCoord(-11, 30),
            new GeoCoord(-8, 29), new GeoCoord(-3, 29),
        }));

        // Asia
        zones.Add(MakeCountry("Russia (Asian)", "Asia", new[]
        {
            new GeoCoord(77, 60), new GeoCoord(77, 180),
            new GeoCoord(65, 180), new GeoCoord(55, 163),
            new GeoCoord(50, 140), new GeoCoord(43, 132),
            new GeoCoord(50, 80), new GeoCoord(50, 60),
            new GeoCoord(55, 60), new GeoCoord(65, 60),
            new GeoCoord(70, 60),
        }));

        zones.Add(MakeCountry("China", "Asia", new[]
        {
            new GeoCoord(50, 80), new GeoCoord(50, 128),
            new GeoCoord(43, 132), new GeoCoord(40, 124),
            new GeoCoord(35, 120), new GeoCoord(25, 120),
            new GeoCoord(22, 108), new GeoCoord(21, 100),
            new GeoCoord(28, 87), new GeoCoord(35, 74),
            new GeoCoord(40, 74), new GeoCoord(45, 80),
        }));

        zones.Add(MakeCountry("India", "Asia", new[]
        {
            new GeoCoord(35, 74), new GeoCoord(32, 77),
            new GeoCoord(28, 88), new GeoCoord(26, 92),
            new GeoCoord(22, 90), new GeoCoord(21, 87),
            new GeoCoord(16, 82), new GeoCoord(8, 77),
            new GeoCoord(12, 74), new GeoCoord(20, 73),
            new GeoCoord(23, 68), new GeoCoord(25, 62),
            new GeoCoord(30, 66), new GeoCoord(34, 72),
        }));

        zones.Add(MakeCountry("Japan", "Asia", new[]
        {
            new GeoCoord(45, 141), new GeoCoord(45, 146),
            new GeoCoord(43, 146), new GeoCoord(38, 142),
            new GeoCoord(35, 141), new GeoCoord(33, 140),
            new GeoCoord(31, 131), new GeoCoord(33, 130),
            new GeoCoord(35, 132), new GeoCoord(37, 137),
            new GeoCoord(40, 140), new GeoCoord(42, 140),
        }));

        zones.Add(MakeCountry("Saudi Arabia", "Asia", new[]
        {
            new GeoCoord(32, 36), new GeoCoord(29, 47),
            new GeoCoord(28, 49), new GeoCoord(24, 51),
            new GeoCoord(20, 55), new GeoCoord(16, 52),
            new GeoCoord(13, 43), new GeoCoord(16, 42),
            new GeoCoord(18, 40), new GeoCoord(28, 37),
        }));

        zones.Add(MakeCountry("Iran", "Asia", new[]
        {
            new GeoCoord(40, 44), new GeoCoord(39, 48),
            new GeoCoord(37, 54), new GeoCoord(37, 61),
            new GeoCoord(32, 61), new GeoCoord(26, 61),
            new GeoCoord(25, 57), new GeoCoord(27, 52),
            new GeoCoord(30, 48), new GeoCoord(33, 44),
            new GeoCoord(37, 44),
        }));

        zones.Add(MakeCountry("Indonesia", "Asia", new[]
        {
            new GeoCoord(5, 95), new GeoCoord(2, 99),
            new GeoCoord(-3, 104), new GeoCoord(-7, 106),
            new GeoCoord(-8, 115), new GeoCoord(-8, 140),
            new GeoCoord(-2, 141), new GeoCoord(2, 128),
            new GeoCoord(4, 118), new GeoCoord(6, 106),
        }));

        zones.Add(MakeCountry("Kazakhstan", "Asia", new[]
        {
            new GeoCoord(55, 60), new GeoCoord(54, 69),
            new GeoCoord(51, 77), new GeoCoord(47, 85),
            new GeoCoord(44, 80), new GeoCoord(41, 69),
            new GeoCoord(41, 55), new GeoCoord(43, 51),
            new GeoCoord(46, 50), new GeoCoord(50, 51),
            new GeoCoord(52, 55),
        }));

        zones.Add(MakeCountry("Mongolia", "Asia", new[]
        {
            new GeoCoord(50, 88), new GeoCoord(50, 116),
            new GeoCoord(47, 120), new GeoCoord(44, 115),
            new GeoCoord(42, 107), new GeoCoord(42, 96),
            new GeoCoord(45, 88), new GeoCoord(48, 88),
        }));

        zones.Add(MakeCountry("Turkey", "Asia", new[]
        {
            new GeoCoord(42, 26), new GeoCoord(41, 33),
            new GeoCoord(41, 40), new GeoCoord(40, 44),
            new GeoCoord(37, 44), new GeoCoord(36, 36),
            new GeoCoord(37, 30), new GeoCoord(36, 28),
            new GeoCoord(38, 26), new GeoCoord(40, 26),
        }));

        // Oceania
        zones.Add(MakeCountry("Australia", "Oceania", new[]
        {
            new GeoCoord(-12, 130), new GeoCoord(-14, 142),
            new GeoCoord(-16, 146), new GeoCoord(-24, 153),
            new GeoCoord(-28, 154), new GeoCoord(-37, 150),
            new GeoCoord(-39, 146), new GeoCoord(-37, 140),
            new GeoCoord(-34, 135), new GeoCoord(-32, 115),
            new GeoCoord(-22, 114), new GeoCoord(-14, 127),
        }));

        zones.Add(MakeCountry("New Zealand", "Oceania", new[]
        {
            new GeoCoord(-34, 173), new GeoCoord(-37, 178),
            new GeoCoord(-39, 178), new GeoCoord(-42, 174),
            new GeoCoord(-46, 170), new GeoCoord(-47, 167),
            new GeoCoord(-44, 166), new GeoCoord(-41, 172),
            new GeoCoord(-38, 175), new GeoCoord(-35, 174),
        }));
    }

    private static Zone MakeCountry(string name, string parent, GeoCoord[] boundary) => new()
    {
        Name = name,
        Type = ZoneType.Country,
        ParentName = parent,
        Boundary = boundary,
    };
}
