namespace Gro.EarthModel;

internal static class CountryData
{
    public static void Add(List<Zone> zones)
    {
        // North America - Canada (5 regions)
        zones.Add(MakeCountry("Canada (Atlantic)", "North America", new[]
        {
            new GeoCoord(53, -67), new GeoCoord(53, -52),
            new GeoCoord(47, -52), new GeoCoord(44, -60),
            new GeoCoord(44, -67), new GeoCoord(45, -67),
            new GeoCoord(48, -67),
        }));

        zones.Add(MakeCountry("Canada (Quebec-Ontario)", "North America", new[]
        {
            new GeoCoord(55, -80), new GeoCoord(55, -67),
            new GeoCoord(53, -67), new GeoCoord(48, -67),
            new GeoCoord(45, -67), new GeoCoord(44, -76),
            new GeoCoord(42, -83), new GeoCoord(46, -83),
            new GeoCoord(49, -90), new GeoCoord(51, -80),
        }));

        zones.Add(MakeCountry("Canada (Prairies)", "North America", new[]
        {
            new GeoCoord(60, -120), new GeoCoord(60, -90),
            new GeoCoord(55, -80), new GeoCoord(51, -80),
            new GeoCoord(49, -90), new GeoCoord(49, -120),
            new GeoCoord(54, -120),
        }));

        zones.Add(MakeCountry("Canada (British Columbia)", "North America", new[]
        {
            new GeoCoord(60, -139), new GeoCoord(60, -120),
            new GeoCoord(54, -120), new GeoCoord(49, -120),
            new GeoCoord(49, -123), new GeoCoord(54, -130),
            new GeoCoord(57, -135),
        }));

        zones.Add(MakeCountry("Canada (North)", "North America", new[]
        {
            new GeoCoord(83, -141), new GeoCoord(83, -52),
            new GeoCoord(72, -52), new GeoCoord(60, -52),
            new GeoCoord(53, -52), new GeoCoord(53, -67),
            new GeoCoord(55, -67), new GeoCoord(55, -80),
            new GeoCoord(60, -90), new GeoCoord(60, -120),
            new GeoCoord(60, -139), new GeoCoord(69, -141),
        }));

        // North America - United States (4 regions)
        zones.Add(MakeCountry("United States (Northeast)", "North America", new[]
        {
            new GeoCoord(49, -90), new GeoCoord(46, -83),
            new GeoCoord(42, -83), new GeoCoord(44, -76),
            new GeoCoord(41, -72), new GeoCoord(35, -75),
            new GeoCoord(35, -80), new GeoCoord(35, -90),
            new GeoCoord(37, -90), new GeoCoord(42, -90),
            new GeoCoord(46, -90),
        }));

        zones.Add(MakeCountry("United States (Southeast)", "North America", new[]
        {
            new GeoCoord(35, -90), new GeoCoord(35, -80),
            new GeoCoord(35, -75), new GeoCoord(30, -81),
            new GeoCoord(25, -80), new GeoCoord(25, -82),
            new GeoCoord(29, -90), new GeoCoord(29, -95),
            new GeoCoord(30, -95), new GeoCoord(33, -95),
            new GeoCoord(37, -90),
        }));

        zones.Add(MakeCountry("United States (Central)", "North America", new[]
        {
            new GeoCoord(49, -104), new GeoCoord(49, -90),
            new GeoCoord(46, -90), new GeoCoord(42, -90),
            new GeoCoord(37, -90), new GeoCoord(33, -95),
            new GeoCoord(30, -95), new GeoCoord(26, -97),
            new GeoCoord(32, -106), new GeoCoord(37, -104),
            new GeoCoord(42, -104),
        }));

        zones.Add(MakeCountry("United States (West)", "North America", new[]
        {
            new GeoCoord(49, -123), new GeoCoord(49, -120),
            new GeoCoord(49, -104), new GeoCoord(42, -104),
            new GeoCoord(37, -104), new GeoCoord(32, -106),
            new GeoCoord(32, -117), new GeoCoord(38, -122),
            new GeoCoord(46, -124),
        }));

        // North America - Alaska
        zones.Add(MakeCountry("United States (Alaska)", "North America", new[]
        {
            new GeoCoord(72, -170), new GeoCoord(72, -141),
            new GeoCoord(69, -141), new GeoCoord(60, -139),
            new GeoCoord(57, -135), new GeoCoord(54, -130),
            new GeoCoord(55, -162), new GeoCoord(58, -168),
            new GeoCoord(65, -168),
        }));

        // Central America - Mexico (3 regions)
        zones.Add(MakeCountry("Mexico (North)", "North America", new[]
        {
            new GeoCoord(32, -117), new GeoCoord(32, -106),
            new GeoCoord(26, -97), new GeoCoord(24, -99),
            new GeoCoord(24, -105), new GeoCoord(23, -110),
            new GeoCoord(28, -112), new GeoCoord(31, -113),
            new GeoCoord(32, -115),
        }));

        zones.Add(MakeCountry("Mexico (Central)", "North America", new[]
        {
            new GeoCoord(24, -105), new GeoCoord(24, -99),
            new GeoCoord(21, -97), new GeoCoord(19, -99),
            new GeoCoord(21, -105),
        }));

        zones.Add(MakeCountry("Mexico (South)", "North America", new[]
        {
            new GeoCoord(21, -105), new GeoCoord(19, -99),
            new GeoCoord(16, -98), new GeoCoord(15, -92),
            new GeoCoord(16, -90), new GeoCoord(18.5, -88),
            new GeoCoord(18.5, -91.5), new GeoCoord(21, -90),
            new GeoCoord(21, -105),
        }));

        // Central America - Guatemala & Belize
        zones.Add(MakeCountry("Guatemala-Belize", "North America", new[]
        {
            new GeoCoord(18.5, -91.5), new GeoCoord(18.5, -88),
            new GeoCoord(16, -88), new GeoCoord(14, -89.5),
            new GeoCoord(14, -92), new GeoCoord(15, -92),
            new GeoCoord(16, -91.5),
        }));

        // Central America - Honduras, El Salvador, Nicaragua
        zones.Add(MakeCountry("Honduras-El Salvador-Nicaragua", "North America", new[]
        {
            new GeoCoord(16, -89.5), new GeoCoord(16, -83),
            new GeoCoord(15, -83), new GeoCoord(12.5, -83.5),
            new GeoCoord(11, -84), new GeoCoord(11, -87.5),
            new GeoCoord(13, -90), new GeoCoord(14, -89.5),
        }));

        // Central America - Costa Rica & Panama
        zones.Add(MakeCountry("Costa Rica-Panama", "North America", new[]
        {
            new GeoCoord(11, -84), new GeoCoord(11, -83),
            new GeoCoord(10, -82), new GeoCoord(9.5, -79),
            new GeoCoord(8, -77), new GeoCoord(7, -77),
            new GeoCoord(7.5, -83), new GeoCoord(9.5, -84.5),
        }));

        // North America - Caribbean
        zones.Add(MakeCountry("Cuba", "North America", new[]
        {
            new GeoCoord(23.5, -84.5), new GeoCoord(23.5, -74),
            new GeoCoord(20, -74), new GeoCoord(20, -84.5),
        }));

        zones.Add(MakeCountry("Haiti-Dominican Republic", "North America", new[]
        {
            new GeoCoord(20, -74.5), new GeoCoord(20, -68),
            new GeoCoord(18, -68), new GeoCoord(18, -74.5),
        }));

        zones.Add(MakeCountry("Jamaica", "North America", new[]
        {
            new GeoCoord(18.5, -78.5), new GeoCoord(18.5, -76),
            new GeoCoord(17.5, -76), new GeoCoord(17.5, -78.5),
        }));

        zones.Add(MakeCountry("Greenland", "North America", new[]
        {
            new GeoCoord(83, -52), new GeoCoord(83, -12),
            new GeoCoord(76, -18), new GeoCoord(70, -22),
            new GeoCoord(65, -54), new GeoCoord(60, -45),
            new GeoCoord(60, -52), new GeoCoord(72, -52),
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
            new GeoCoord(68, 16), new GeoCoord(64, 14),
            new GeoCoord(62, 12), new GeoCoord(60, 12),
            new GeoCoord(58, 8), new GeoCoord(58, 5),
            new GeoCoord(60, 4), new GeoCoord(62, 4),
            new GeoCoord(64, 8), new GeoCoord(67, 14),
            new GeoCoord(69, 18),
        }));

        zones.Add(MakeCountry("Iceland", "Europe", new[]
        {
            new GeoCoord(66.5, -24), new GeoCoord(66.5, -13),
            new GeoCoord(65, -13), new GeoCoord(63.5, -14),
            new GeoCoord(63.5, -20), new GeoCoord(64, -24),
        }));

        zones.Add(MakeCountry("Ireland", "Europe", new[]
        {
            new GeoCoord(55.5, -10.5), new GeoCoord(55.5, -5.5),
            new GeoCoord(53.5, -6), new GeoCoord(51.5, -6),
            new GeoCoord(51.5, -10), new GeoCoord(53, -10.5),
        }));

        zones.Add(MakeCountry("Portugal", "Europe", new[]
        {
            new GeoCoord(42, -9.5), new GeoCoord(42, -6.5),
            new GeoCoord(40, -7), new GeoCoord(38.5, -7),
            new GeoCoord(37, -7.5), new GeoCoord(37, -9),
            new GeoCoord(38.5, -9.5), new GeoCoord(40, -9),
        }));

        zones.Add(MakeCountry("Finland", "Europe", new[]
        {
            new GeoCoord(70, 25), new GeoCoord(70, 30),
            new GeoCoord(65, 30), new GeoCoord(61, 30),
            new GeoCoord(60, 24), new GeoCoord(60, 21),
            new GeoCoord(63, 21), new GeoCoord(66, 24),
            new GeoCoord(68, 22),
        }));

        zones.Add(MakeCountry("Denmark", "Europe", new[]
        {
            new GeoCoord(57.8, 8), new GeoCoord(57.8, 13),
            new GeoCoord(55, 13), new GeoCoord(54.5, 12),
            new GeoCoord(54.5, 8.5), new GeoCoord(55, 8),
            new GeoCoord(56, 8),
        }));

        zones.Add(MakeCountry("Netherlands", "Europe", new[]
        {
            new GeoCoord(53.5, 4), new GeoCoord(53.5, 7),
            new GeoCoord(52, 7), new GeoCoord(51.5, 6),
            new GeoCoord(51.5, 3.5), new GeoCoord(52, 4),
        }));

        zones.Add(MakeCountry("Belgium", "Europe", new[]
        {
            new GeoCoord(51.5, 2.5), new GeoCoord(51.5, 6),
            new GeoCoord(50, 6.5), new GeoCoord(49.5, 6),
            new GeoCoord(49.5, 3), new GeoCoord(50.5, 2.5),
        }));

        zones.Add(MakeCountry("Switzerland", "Europe", new[]
        {
            new GeoCoord(47.8, 6), new GeoCoord(47.8, 10.5),
            new GeoCoord(47, 10.5), new GeoCoord(46, 10),
            new GeoCoord(46, 6), new GeoCoord(47, 6),
        }));

        zones.Add(MakeCountry("Austria", "Europe", new[]
        {
            new GeoCoord(49, 10), new GeoCoord(49, 17),
            new GeoCoord(48, 17), new GeoCoord(47, 16),
            new GeoCoord(46.5, 13), new GeoCoord(47, 10),
            new GeoCoord(47.5, 10),
        }));

        zones.Add(MakeCountry("Czech Republic", "Europe", new[]
        {
            new GeoCoord(51, 12), new GeoCoord(51, 18.5),
            new GeoCoord(50, 18.5), new GeoCoord(49, 18),
            new GeoCoord(48.5, 14), new GeoCoord(49, 12),
            new GeoCoord(50, 12),
        }));

        zones.Add(MakeCountry("Slovakia", "Europe", new[]
        {
            new GeoCoord(49.5, 17), new GeoCoord(49.5, 22.5),
            new GeoCoord(48.5, 22), new GeoCoord(48, 20),
            new GeoCoord(47.5, 17), new GeoCoord(48.5, 17),
        }));

        zones.Add(MakeCountry("Hungary", "Europe", new[]
        {
            new GeoCoord(48.5, 16), new GeoCoord(48.5, 23),
            new GeoCoord(47, 23), new GeoCoord(46, 21),
            new GeoCoord(45.5, 18), new GeoCoord(46, 16),
            new GeoCoord(47, 16),
        }));

        zones.Add(MakeCountry("Romania", "Europe", new[]
        {
            new GeoCoord(48, 22), new GeoCoord(48, 28),
            new GeoCoord(46, 30), new GeoCoord(44, 29),
            new GeoCoord(44, 23), new GeoCoord(44.5, 22),
            new GeoCoord(46, 21), new GeoCoord(47.5, 22),
        }));

        zones.Add(MakeCountry("Bulgaria", "Europe", new[]
        {
            new GeoCoord(44, 22.5), new GeoCoord(44, 29),
            new GeoCoord(43, 28.5), new GeoCoord(42, 28),
            new GeoCoord(41.5, 24), new GeoCoord(42, 22.5),
            new GeoCoord(43, 22.5),
        }));

        zones.Add(MakeCountry("Greece", "Europe", new[]
        {
            new GeoCoord(41.5, 20), new GeoCoord(41.5, 26.5),
            new GeoCoord(39, 26), new GeoCoord(37.5, 24),
            new GeoCoord(35, 24), new GeoCoord(35, 22),
            new GeoCoord(37, 21), new GeoCoord(38.5, 20),
            new GeoCoord(39.5, 20),
        }));

        zones.Add(MakeCountry("Serbia", "Europe", new[]
        {
            new GeoCoord(46, 19), new GeoCoord(46, 23),
            new GeoCoord(44.5, 22.5), new GeoCoord(43, 23),
            new GeoCoord(42, 22), new GeoCoord(42.5, 20),
            new GeoCoord(44, 19), new GeoCoord(45, 19),
        }));

        zones.Add(MakeCountry("Croatia", "Europe", new[]
        {
            new GeoCoord(46.5, 13.5), new GeoCoord(46.5, 19),
            new GeoCoord(45, 19), new GeoCoord(44.5, 17),
            new GeoCoord(43, 16), new GeoCoord(42.5, 16),
            new GeoCoord(43, 14), new GeoCoord(45, 13.5),
        }));

        zones.Add(MakeCountry("Bosnia and Herzegovina", "Europe", new[]
        {
            new GeoCoord(45, 15.5), new GeoCoord(45, 19.5),
            new GeoCoord(44, 19.5), new GeoCoord(43, 19),
            new GeoCoord(42.5, 17.5), new GeoCoord(43, 16),
            new GeoCoord(44, 15.5),
        }));

        zones.Add(MakeCountry("Montenegro", "Europe", new[]
        {
            new GeoCoord(43.5, 18.5), new GeoCoord(43.5, 20.5),
            new GeoCoord(42.5, 20), new GeoCoord(42, 19),
            new GeoCoord(42, 18.5), new GeoCoord(42.5, 18.5),
        }));

        zones.Add(MakeCountry("North Macedonia", "Europe", new[]
        {
            new GeoCoord(42, 20), new GeoCoord(42, 23),
            new GeoCoord(41, 23), new GeoCoord(40.5, 21),
            new GeoCoord(41, 20), new GeoCoord(41.5, 20),
        }));

        zones.Add(MakeCountry("Albania", "Europe", new[]
        {
            new GeoCoord(42.5, 19), new GeoCoord(42.5, 21),
            new GeoCoord(41, 21), new GeoCoord(40, 20.5),
            new GeoCoord(39.5, 20), new GeoCoord(40.5, 19),
            new GeoCoord(41.5, 19),
        }));

        zones.Add(MakeCountry("Slovenia", "Europe", new[]
        {
            new GeoCoord(46.8, 13.5), new GeoCoord(46.8, 16.5),
            new GeoCoord(46, 16.5), new GeoCoord(45.5, 15),
            new GeoCoord(45.5, 13.5), new GeoCoord(46, 13.5),
        }));

        zones.Add(MakeCountry("Belarus", "Europe", new[]
        {
            new GeoCoord(56.5, 23.5), new GeoCoord(56.5, 32.5),
            new GeoCoord(54, 32), new GeoCoord(52, 31),
            new GeoCoord(51.5, 24), new GeoCoord(53, 23.5),
            new GeoCoord(55, 24),
        }));

        zones.Add(MakeCountry("Moldova", "Europe", new[]
        {
            new GeoCoord(48.5, 27), new GeoCoord(48.5, 30),
            new GeoCoord(47, 30), new GeoCoord(46, 30),
            new GeoCoord(46, 28), new GeoCoord(47, 27),
        }));

        zones.Add(MakeCountry("Estonia", "Europe", new[]
        {
            new GeoCoord(59.5, 22), new GeoCoord(59.5, 28),
            new GeoCoord(58, 28), new GeoCoord(57.5, 24),
            new GeoCoord(57.5, 22), new GeoCoord(58.5, 22),
        }));

        zones.Add(MakeCountry("Latvia", "Europe", new[]
        {
            new GeoCoord(58, 21), new GeoCoord(58, 28.5),
            new GeoCoord(57, 28), new GeoCoord(56, 28),
            new GeoCoord(55.5, 24), new GeoCoord(56, 21),
            new GeoCoord(57, 21),
        }));

        zones.Add(MakeCountry("Lithuania", "Europe", new[]
        {
            new GeoCoord(56.5, 21), new GeoCoord(56.5, 26.5),
            new GeoCoord(55, 26.5), new GeoCoord(54, 26),
            new GeoCoord(54, 22), new GeoCoord(55, 21),
        }));

        // Africa - North
        zones.Add(MakeCountry("Morocco", "Africa", new[]
        {
            new GeoCoord(36, -6), new GeoCoord(36, -1),
            new GeoCoord(35, -2), new GeoCoord(32, -2),
            new GeoCoord(27, -9), new GeoCoord(21, -17),
            new GeoCoord(25, -17), new GeoCoord(29, -13),
            new GeoCoord(33, -8),
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

        zones.Add(MakeCountry("Tunisia", "Africa", new[]
        {
            new GeoCoord(37.5, 7.5), new GeoCoord(37.5, 11.5),
            new GeoCoord(34, 11.5), new GeoCoord(30, 10),
            new GeoCoord(30, 7.5), new GeoCoord(34, 7.5),
        }));

        zones.Add(MakeCountry("Libya", "Africa", new[]
        {
            new GeoCoord(34, 11), new GeoCoord(33, 25),
            new GeoCoord(30, 25), new GeoCoord(22, 24),
            new GeoCoord(20, 15), new GeoCoord(23, 11),
            new GeoCoord(30, 10),
        }));

        zones.Add(MakeCountry("Egypt", "Africa", new[]
        {
            new GeoCoord(31, 25), new GeoCoord(31, 34),
            new GeoCoord(29, 35), new GeoCoord(22, 37),
            new GeoCoord(22, 25), new GeoCoord(25, 25),
            new GeoCoord(29, 25),
        }));

        // Africa - West
        zones.Add(MakeCountry("Mauritania", "Africa", new[]
        {
            new GeoCoord(21, -17), new GeoCoord(21, -5),
            new GeoCoord(14.5, -5), new GeoCoord(14.5, -12),
            new GeoCoord(16, -16), new GeoCoord(19, -17),
        }));

        zones.Add(MakeCountry("Mali", "Africa", new[]
        {
            new GeoCoord(25, -12), new GeoCoord(25, 4),
            new GeoCoord(15, 4), new GeoCoord(10, 0),
            new GeoCoord(10, -5), new GeoCoord(12, -12),
        }));

        zones.Add(MakeCountry("Niger", "Africa", new[]
        {
            new GeoCoord(24, 0), new GeoCoord(24, 16),
            new GeoCoord(13, 16), new GeoCoord(11.5, 14),
            new GeoCoord(11.5, 0), new GeoCoord(15, 0),
        }));

        zones.Add(MakeCountry("Senegal", "Africa", new[]
        {
            new GeoCoord(17, -17.5), new GeoCoord(17, -11.5),
            new GeoCoord(14, -11.5), new GeoCoord(12.3, -12),
            new GeoCoord(12.3, -16.5), new GeoCoord(14.5, -17.5),
        }));

        zones.Add(MakeCountry("Gambia", "Africa", new[]
        {
            new GeoCoord(13.9, -17), new GeoCoord(13.9, -13.5),
            new GeoCoord(13.0, -13.5), new GeoCoord(13.0, -17),
        }));

        zones.Add(MakeCountry("Guinea-Bissau", "Africa", new[]
        {
            new GeoCoord(12.7, -16.5), new GeoCoord(12.7, -13.5),
            new GeoCoord(10.8, -13.5), new GeoCoord(10.8, -16.5),
        }));

        zones.Add(MakeCountry("Guinea", "Africa", new[]
        {
            new GeoCoord(12.5, -15), new GeoCoord(12.5, -7.5),
            new GeoCoord(7.2, -7.5), new GeoCoord(7.2, -13),
            new GeoCoord(9, -15),
        }));

        zones.Add(MakeCountry("Sierra Leone", "Africa", new[]
        {
            new GeoCoord(10, -13.5), new GeoCoord(10, -10),
            new GeoCoord(6.8, -10), new GeoCoord(6.8, -13.5),
        }));

        zones.Add(MakeCountry("Liberia", "Africa", new[]
        {
            new GeoCoord(8.5, -11.5), new GeoCoord(8.5, -7.5),
            new GeoCoord(4.3, -7.5), new GeoCoord(4.3, -11.5),
        }));

        zones.Add(MakeCountry("Ivory Coast", "Africa", new[]
        {
            new GeoCoord(10.7, -8.5), new GeoCoord(10.7, -2.5),
            new GeoCoord(4.3, -2.5), new GeoCoord(4.3, -8.5),
        }));

        zones.Add(MakeCountry("Burkina Faso", "Africa", new[]
        {
            new GeoCoord(15, -5.5), new GeoCoord(15, 2.5),
            new GeoCoord(9.5, 2.5), new GeoCoord(9.5, -5.5),
        }));

        zones.Add(MakeCountry("Ghana", "Africa", new[]
        {
            new GeoCoord(11, -3.3), new GeoCoord(11, 1.2),
            new GeoCoord(4.7, 1.2), new GeoCoord(4.7, -3.3),
        }));

        zones.Add(MakeCountry("Togo", "Africa", new[]
        {
            new GeoCoord(11.2, -0.1), new GeoCoord(11.2, 1.9),
            new GeoCoord(5.8, 1.9), new GeoCoord(5.8, -0.1),
        }));

        zones.Add(MakeCountry("Benin", "Africa", new[]
        {
            new GeoCoord(12.5, 1), new GeoCoord(12.5, 4),
            new GeoCoord(6, 4), new GeoCoord(6, 1),
        }));

        zones.Add(MakeCountry("Nigeria", "Africa", new[]
        {
            new GeoCoord(14, 3), new GeoCoord(13, 14),
            new GeoCoord(10, 14), new GeoCoord(7, 12),
            new GeoCoord(4, 8), new GeoCoord(4, 3),
            new GeoCoord(7, 3), new GeoCoord(10, 3),
        }));

        // Africa - Central
        zones.Add(MakeCountry("Chad", "Africa", new[]
        {
            new GeoCoord(23.5, 14), new GeoCoord(23.5, 24),
            new GeoCoord(8, 24), new GeoCoord(8, 14),
            new GeoCoord(13, 14),
        }));

        zones.Add(MakeCountry("Cameroon", "Africa", new[]
        {
            new GeoCoord(13, 8.5), new GeoCoord(13, 16),
            new GeoCoord(2, 16), new GeoCoord(2, 8.5),
            new GeoCoord(4, 8.5), new GeoCoord(6, 9),
            new GeoCoord(10, 9),
        }));

        zones.Add(MakeCountry("Central African Republic", "Africa", new[]
        {
            new GeoCoord(11, 14.5), new GeoCoord(11, 27.5),
            new GeoCoord(2.2, 27.5), new GeoCoord(2.2, 14.5),
        }));

        zones.Add(MakeCountry("Equatorial Guinea", "Africa", new[]
        {
            new GeoCoord(4.5, 8), new GeoCoord(4.5, 12),
            new GeoCoord(1, 12), new GeoCoord(1, 8),
        }));

        zones.Add(MakeCountry("Gabon", "Africa", new[]
        {
            new GeoCoord(2.5, 8.5), new GeoCoord(2.5, 14.5),
            new GeoCoord(-4, 14.5), new GeoCoord(-4, 8.5),
        }));

        zones.Add(MakeCountry("Republic of Congo", "Africa", new[]
        {
            new GeoCoord(4, 11), new GeoCoord(4, 18.5),
            new GeoCoord(-5, 18.5), new GeoCoord(-5, 11),
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

        // Africa - East
        zones.Add(MakeCountry("Sudan", "Africa", new[]
        {
            new GeoCoord(22, 24), new GeoCoord(22, 37),
            new GeoCoord(18, 38), new GeoCoord(14, 36),
            new GeoCoord(10, 34), new GeoCoord(10, 24),
            new GeoCoord(15, 24),
        }));

        zones.Add(MakeCountry("South Sudan", "Africa", new[]
        {
            new GeoCoord(12, 24), new GeoCoord(12, 35),
            new GeoCoord(4, 35), new GeoCoord(3.5, 30),
            new GeoCoord(3.5, 24),
        }));

        zones.Add(MakeCountry("Eritrea", "Africa", new[]
        {
            new GeoCoord(18, 36), new GeoCoord(18, 43),
            new GeoCoord(15, 43), new GeoCoord(14, 42),
            new GeoCoord(15, 36),
        }));

        zones.Add(MakeCountry("Djibouti", "Africa", new[]
        {
            new GeoCoord(12.7, 42), new GeoCoord(12.7, 43.5),
            new GeoCoord(11, 43.5), new GeoCoord(11, 42),
        }));

        zones.Add(MakeCountry("Ethiopia", "Africa", new[]
        {
            new GeoCoord(15, 36), new GeoCoord(14, 42),
            new GeoCoord(11, 43), new GeoCoord(8, 47),
            new GeoCoord(4, 42), new GeoCoord(4, 36),
            new GeoCoord(6, 35), new GeoCoord(9, 34),
            new GeoCoord(12, 36),
        }));

        zones.Add(MakeCountry("Somalia", "Africa", new[]
        {
            new GeoCoord(12, 41), new GeoCoord(12, 51),
            new GeoCoord(2, 51), new GeoCoord(-1.5, 42),
            new GeoCoord(5, 41), new GeoCoord(8, 44),
        }));

        zones.Add(MakeCountry("Kenya", "Africa", new[]
        {
            new GeoCoord(5, 34), new GeoCoord(5, 42),
            new GeoCoord(-1, 41), new GeoCoord(-5, 40),
            new GeoCoord(-5, 34),
        }));

        zones.Add(MakeCountry("Uganda", "Africa", new[]
        {
            new GeoCoord(4, 30), new GeoCoord(4, 35),
            new GeoCoord(-1.5, 35), new GeoCoord(-1.5, 29.5),
            new GeoCoord(0, 29.5),
        }));

        zones.Add(MakeCountry("Rwanda", "Africa", new[]
        {
            new GeoCoord(-1, 28.5), new GeoCoord(-1, 30.8),
            new GeoCoord(-3, 30.8), new GeoCoord(-3, 28.5),
        }));

        zones.Add(MakeCountry("Burundi", "Africa", new[]
        {
            new GeoCoord(-2.5, 28.8), new GeoCoord(-2.5, 30.8),
            new GeoCoord(-4.5, 30.8), new GeoCoord(-4.5, 28.8),
        }));

        zones.Add(MakeCountry("Tanzania", "Africa", new[]
        {
            new GeoCoord(-1, 30), new GeoCoord(-1, 37),
            new GeoCoord(-5, 40), new GeoCoord(-10, 40),
            new GeoCoord(-11, 35), new GeoCoord(-11, 30),
            new GeoCoord(-8, 29), new GeoCoord(-3, 29),
        }));

        // Africa - Southern
        zones.Add(MakeCountry("Angola", "Africa", new[]
        {
            new GeoCoord(-5, 12), new GeoCoord(-5, 24),
            new GeoCoord(-18, 24), new GeoCoord(-18, 12),
        }));

        zones.Add(MakeCountry("Zambia", "Africa", new[]
        {
            new GeoCoord(-8, 22), new GeoCoord(-8, 33),
            new GeoCoord(-18, 33), new GeoCoord(-18, 22),
        }));

        zones.Add(MakeCountry("Malawi", "Africa", new[]
        {
            new GeoCoord(-9, 33), new GeoCoord(-9, 36),
            new GeoCoord(-17, 36), new GeoCoord(-17, 33),
        }));

        zones.Add(MakeCountry("Mozambique", "Africa", new[]
        {
            new GeoCoord(-10, 34), new GeoCoord(-10, 41),
            new GeoCoord(-17, 41), new GeoCoord(-27, 35),
            new GeoCoord(-27, 30), new GeoCoord(-22, 31),
            new GeoCoord(-16, 34),
        }));

        zones.Add(MakeCountry("Zimbabwe", "Africa", new[]
        {
            new GeoCoord(-15, 25), new GeoCoord(-15, 33),
            new GeoCoord(-22, 33), new GeoCoord(-22, 25),
        }));

        zones.Add(MakeCountry("Namibia", "Africa", new[]
        {
            new GeoCoord(-17, 12), new GeoCoord(-17, 21),
            new GeoCoord(-22, 20), new GeoCoord(-29, 20),
            new GeoCoord(-29, 12), new GeoCoord(-22, 12),
        }));

        zones.Add(MakeCountry("Botswana", "Africa", new[]
        {
            new GeoCoord(-18, 20), new GeoCoord(-18, 29),
            new GeoCoord(-27, 29), new GeoCoord(-27, 20),
        }));

        zones.Add(MakeCountry("South Africa", "Africa", new[]
        {
            new GeoCoord(-22, 17), new GeoCoord(-22, 31),
            new GeoCoord(-27, 32), new GeoCoord(-30, 31),
            new GeoCoord(-34, 26), new GeoCoord(-34, 18),
            new GeoCoord(-31, 17), new GeoCoord(-28, 17),
        }));

        zones.Add(MakeCountry("Lesotho", "Africa", new[]
        {
            new GeoCoord(-28.5, 27), new GeoCoord(-28.5, 30),
            new GeoCoord(-30.5, 30), new GeoCoord(-30.5, 27),
        }));

        zones.Add(MakeCountry("Eswatini", "Africa", new[]
        {
            new GeoCoord(-25.5, 30.5), new GeoCoord(-25.5, 32.5),
            new GeoCoord(-27.5, 32.5), new GeoCoord(-27.5, 30.5),
        }));

        zones.Add(MakeCountry("Madagascar", "Africa", new[]
        {
            new GeoCoord(-12, 43), new GeoCoord(-12, 50),
            new GeoCoord(-16, 50), new GeoCoord(-26, 47),
            new GeoCoord(-26, 43), new GeoCoord(-20, 43),
        }));

        // Asia - Russia (3 regions)
        zones.Add(MakeCountry("Russia (West Siberia)", "Asia", new[]
        {
            new GeoCoord(77, 60), new GeoCoord(77, 100),
            new GeoCoord(70, 100), new GeoCoord(55, 100),
            new GeoCoord(50, 80), new GeoCoord(50, 60),
            new GeoCoord(55, 60), new GeoCoord(65, 60),
            new GeoCoord(70, 60),
        }));

        zones.Add(MakeCountry("Russia (East Siberia)", "Asia", new[]
        {
            new GeoCoord(77, 100), new GeoCoord(77, 140),
            new GeoCoord(70, 140), new GeoCoord(60, 140),
            new GeoCoord(50, 130), new GeoCoord(50, 100),
            new GeoCoord(55, 100), new GeoCoord(70, 100),
        }));

        zones.Add(MakeCountry("Russia (Far East)", "Asia", new[]
        {
            new GeoCoord(77, 140), new GeoCoord(77, 180),
            new GeoCoord(65, 180), new GeoCoord(55, 163),
            new GeoCoord(50, 140), new GeoCoord(43, 132),
            new GeoCoord(43, 130), new GeoCoord(50, 130),
            new GeoCoord(60, 140), new GeoCoord(70, 140),
        }));

        // Asia - China (3 regions)
        zones.Add(MakeCountry("China (West)", "Asia", new[]
        {
            new GeoCoord(50, 80), new GeoCoord(50, 100),
            new GeoCoord(42, 96), new GeoCoord(42, 90),
            new GeoCoord(35, 90), new GeoCoord(28, 87),
            new GeoCoord(35, 74), new GeoCoord(40, 74),
            new GeoCoord(45, 80),
        }));

        zones.Add(MakeCountry("China (North)", "Asia", new[]
        {
            new GeoCoord(50, 100), new GeoCoord(50, 128),
            new GeoCoord(43, 132), new GeoCoord(40, 124),
            new GeoCoord(35, 120), new GeoCoord(32, 122),
            new GeoCoord(32, 105), new GeoCoord(35, 90),
            new GeoCoord(42, 90), new GeoCoord(42, 96),
        }));

        zones.Add(MakeCountry("China (South)", "Asia", new[]
        {
            new GeoCoord(35, 90), new GeoCoord(32, 105),
            new GeoCoord(32, 122), new GeoCoord(25, 120),
            new GeoCoord(22, 108), new GeoCoord(21, 100),
            new GeoCoord(28, 87),
        }));

        // Asia - Mongolia
        zones.Add(MakeCountry("Mongolia", "Asia", new[]
        {
            new GeoCoord(50, 88), new GeoCoord(50, 116),
            new GeoCoord(47, 120), new GeoCoord(44, 115),
            new GeoCoord(42, 107), new GeoCoord(42, 96),
            new GeoCoord(45, 88), new GeoCoord(48, 88),
        }));

        // Asia - East Asia
        zones.Add(MakeCountry("Japan", "Asia", new[]
        {
            new GeoCoord(45, 141), new GeoCoord(45, 146),
            new GeoCoord(43, 146), new GeoCoord(38, 142),
            new GeoCoord(35, 141), new GeoCoord(33, 140),
            new GeoCoord(31, 131), new GeoCoord(33, 130),
            new GeoCoord(35, 132), new GeoCoord(37, 137),
            new GeoCoord(40, 140), new GeoCoord(42, 140),
        }));

        zones.Add(MakeCountry("South Korea", "Asia", new[]
        {
            new GeoCoord(38, 126), new GeoCoord(38, 130),
            new GeoCoord(35, 130), new GeoCoord(34, 127),
            new GeoCoord(35, 126), new GeoCoord(37, 126),
        }));

        zones.Add(MakeCountry("North Korea", "Asia", new[]
        {
            new GeoCoord(43, 124), new GeoCoord(43, 130),
            new GeoCoord(40, 130), new GeoCoord(38, 130),
            new GeoCoord(38, 126), new GeoCoord(40, 124),
        }));

        zones.Add(MakeCountry("Taiwan", "Asia", new[]
        {
            new GeoCoord(25.5, 120), new GeoCoord(25.5, 122),
            new GeoCoord(23, 122), new GeoCoord(22, 120.5),
            new GeoCoord(23, 120), new GeoCoord(24, 120),
        }));

        // Asia - Southeast Asia
        zones.Add(MakeCountry("Vietnam", "Asia", new[]
        {
            new GeoCoord(23, 103), new GeoCoord(22, 108),
            new GeoCoord(17, 108), new GeoCoord(11, 109),
            new GeoCoord(8.5, 105), new GeoCoord(10, 104),
            new GeoCoord(14, 108), new GeoCoord(18, 104),
            new GeoCoord(21, 103),
        }));

        zones.Add(MakeCountry("Thailand", "Asia", new[]
        {
            new GeoCoord(21, 98), new GeoCoord(20, 104),
            new GeoCoord(18, 104), new GeoCoord(14, 105),
            new GeoCoord(13, 101), new GeoCoord(10, 99),
            new GeoCoord(6, 100), new GeoCoord(8, 98),
            new GeoCoord(14, 98), new GeoCoord(18, 98),
        }));

        zones.Add(MakeCountry("Myanmar", "Asia", new[]
        {
            new GeoCoord(28, 96), new GeoCoord(26, 98),
            new GeoCoord(21, 100), new GeoCoord(21, 98),
            new GeoCoord(18, 98), new GeoCoord(14, 98),
            new GeoCoord(10, 98), new GeoCoord(16, 94),
            new GeoCoord(20, 92), new GeoCoord(26, 92),
            new GeoCoord(28, 94),
        }));

        zones.Add(MakeCountry("Cambodia", "Asia", new[]
        {
            new GeoCoord(14, 103), new GeoCoord(14, 108),
            new GeoCoord(10, 107), new GeoCoord(10, 103),
            new GeoCoord(11, 103),
        }));

        zones.Add(MakeCountry("Laos", "Asia", new[]
        {
            new GeoCoord(22, 100), new GeoCoord(22, 103),
            new GeoCoord(18, 104), new GeoCoord(14, 108),
            new GeoCoord(14, 105), new GeoCoord(14, 103),
            new GeoCoord(18, 100), new GeoCoord(20, 100),
        }));

        zones.Add(MakeCountry("Malaysia", "Asia", new[]
        {
            new GeoCoord(7, 100), new GeoCoord(7, 104),
            new GeoCoord(4, 104), new GeoCoord(1, 104),
            new GeoCoord(1, 100), new GeoCoord(3, 100),
            new GeoCoord(5, 100),
        }));

        zones.Add(MakeCountry("Philippines", "Asia", new[]
        {
            new GeoCoord(19, 120), new GeoCoord(19, 126),
            new GeoCoord(14, 126), new GeoCoord(10, 126),
            new GeoCoord(6, 125), new GeoCoord(6, 120),
            new GeoCoord(10, 119), new GeoCoord(14, 120),
            new GeoCoord(16, 120),
        }));

        zones.Add(MakeCountry("Indonesia", "Asia", new[]
        {
            new GeoCoord(5, 95), new GeoCoord(2, 99),
            new GeoCoord(-3, 104), new GeoCoord(-7, 106),
            new GeoCoord(-8, 115), new GeoCoord(-8, 140),
            new GeoCoord(-2, 141), new GeoCoord(2, 128),
            new GeoCoord(4, 118), new GeoCoord(6, 106),
        }));

        // Asia - South Asia
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

        zones.Add(MakeCountry("Pakistan", "Asia", new[]
        {
            new GeoCoord(37, 71), new GeoCoord(36, 76),
            new GeoCoord(34, 74), new GeoCoord(30, 72),
            new GeoCoord(25, 68), new GeoCoord(24, 66),
            new GeoCoord(25, 62), new GeoCoord(27, 63),
            new GeoCoord(30, 61), new GeoCoord(32, 61),
            new GeoCoord(35, 64), new GeoCoord(37, 67),
        }));

        zones.Add(MakeCountry("Bangladesh", "Asia", new[]
        {
            new GeoCoord(26, 88), new GeoCoord(26, 92),
            new GeoCoord(22, 92), new GeoCoord(21, 92),
            new GeoCoord(21, 89), new GeoCoord(22, 88),
            new GeoCoord(24, 88),
        }));

        zones.Add(MakeCountry("Sri Lanka", "Asia", new[]
        {
            new GeoCoord(10, 79.5), new GeoCoord(10, 82),
            new GeoCoord(7, 82), new GeoCoord(6, 80),
            new GeoCoord(7, 79.5), new GeoCoord(9, 79.5),
        }));

        zones.Add(MakeCountry("Nepal", "Asia", new[]
        {
            new GeoCoord(30, 80), new GeoCoord(30, 88),
            new GeoCoord(28, 88), new GeoCoord(27, 84),
            new GeoCoord(27, 80), new GeoCoord(29, 80),
        }));

        zones.Add(MakeCountry("Bhutan", "Asia", new[]
        {
            new GeoCoord(28, 89), new GeoCoord(28, 92),
            new GeoCoord(27, 92), new GeoCoord(26.5, 90),
            new GeoCoord(27, 89), new GeoCoord(27.5, 89),
        }));

        zones.Add(MakeCountry("Afghanistan", "Asia", new[]
        {
            new GeoCoord(37, 61), new GeoCoord(37, 67),
            new GeoCoord(37, 71), new GeoCoord(36, 72),
            new GeoCoord(35, 71), new GeoCoord(34, 71),
            new GeoCoord(31, 69), new GeoCoord(30, 61),
            new GeoCoord(32, 61),
        }));

        // Asia - Central Asia
        zones.Add(MakeCountry("Kazakhstan", "Asia", new[]
        {
            new GeoCoord(55, 60), new GeoCoord(54, 69),
            new GeoCoord(51, 77), new GeoCoord(47, 85),
            new GeoCoord(44, 80), new GeoCoord(41, 69),
            new GeoCoord(41, 55), new GeoCoord(43, 51),
            new GeoCoord(46, 50), new GeoCoord(50, 51),
            new GeoCoord(52, 55),
        }));

        zones.Add(MakeCountry("Uzbekistan", "Asia", new[]
        {
            new GeoCoord(45, 56), new GeoCoord(45, 68),
            new GeoCoord(42, 71), new GeoCoord(40, 69),
            new GeoCoord(38, 66), new GeoCoord(37, 61),
            new GeoCoord(41, 55), new GeoCoord(42, 56),
        }));

        zones.Add(MakeCountry("Turkmenistan", "Asia", new[]
        {
            new GeoCoord(42, 53), new GeoCoord(41, 55),
            new GeoCoord(38, 66), new GeoCoord(37, 61),
            new GeoCoord(35, 61), new GeoCoord(36, 54),
            new GeoCoord(38, 53), new GeoCoord(40, 53),
        }));

        zones.Add(MakeCountry("Kyrgyzstan", "Asia", new[]
        {
            new GeoCoord(43, 69), new GeoCoord(43, 80),
            new GeoCoord(40, 80), new GeoCoord(39, 74),
            new GeoCoord(40, 69), new GeoCoord(42, 69),
        }));

        zones.Add(MakeCountry("Tajikistan", "Asia", new[]
        {
            new GeoCoord(41, 67), new GeoCoord(40, 69),
            new GeoCoord(39, 74), new GeoCoord(37, 74),
            new GeoCoord(37, 71), new GeoCoord(37, 67),
            new GeoCoord(38, 67), new GeoCoord(39, 67),
        }));

        // Asia - West Asia / Middle East
        zones.Add(MakeCountry("Turkey", "Asia", new[]
        {
            new GeoCoord(42, 26), new GeoCoord(41, 33),
            new GeoCoord(41, 40), new GeoCoord(40, 44),
            new GeoCoord(37, 44), new GeoCoord(36, 36),
            new GeoCoord(37, 30), new GeoCoord(36, 28),
            new GeoCoord(38, 26), new GeoCoord(40, 26),
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
            new GeoCoord(35, 61), new GeoCoord(32, 61),
            new GeoCoord(26, 61), new GeoCoord(25, 57),
            new GeoCoord(27, 52), new GeoCoord(30, 48),
            new GeoCoord(33, 44), new GeoCoord(37, 44),
        }));

        zones.Add(MakeCountry("Iraq", "Asia", new[]
        {
            new GeoCoord(37, 40), new GeoCoord(37, 44),
            new GeoCoord(35, 46), new GeoCoord(33, 46),
            new GeoCoord(30, 48), new GeoCoord(29, 47),
            new GeoCoord(32, 39), new GeoCoord(34, 38),
            new GeoCoord(36, 40),
        }));

        zones.Add(MakeCountry("Syria", "Asia", new[]
        {
            new GeoCoord(37, 36), new GeoCoord(37, 42),
            new GeoCoord(35, 42), new GeoCoord(33, 42),
            new GeoCoord(33, 36), new GeoCoord(34, 36),
            new GeoCoord(36, 36),
        }));

        zones.Add(MakeCountry("Jordan", "Asia", new[]
        {
            new GeoCoord(33, 35), new GeoCoord(33, 39),
            new GeoCoord(32, 39), new GeoCoord(29, 37),
            new GeoCoord(29, 35), new GeoCoord(31, 35),
        }));

        zones.Add(MakeCountry("Lebanon-Israel", "Asia", new[]
        {
            new GeoCoord(34.5, 35), new GeoCoord(34.5, 36.5),
            new GeoCoord(33, 36.5), new GeoCoord(31, 35.5),
            new GeoCoord(29.5, 35), new GeoCoord(31, 34),
            new GeoCoord(33, 35),
        }));

        zones.Add(MakeCountry("Yemen", "Asia", new[]
        {
            new GeoCoord(18, 42), new GeoCoord(16, 52),
            new GeoCoord(13, 52), new GeoCoord(12, 45),
            new GeoCoord(13, 43), new GeoCoord(15, 42),
        }));

        zones.Add(MakeCountry("Oman", "Asia", new[]
        {
            new GeoCoord(26, 56), new GeoCoord(24, 59),
            new GeoCoord(21, 59), new GeoCoord(17, 54),
            new GeoCoord(16, 52), new GeoCoord(20, 55),
            new GeoCoord(23, 57), new GeoCoord(25, 56),
        }));

        zones.Add(MakeCountry("UAE-Qatar-Kuwait-Bahrain", "Asia", new[]
        {
            new GeoCoord(30, 47), new GeoCoord(29, 47),
            new GeoCoord(28, 49), new GeoCoord(24, 51),
            new GeoCoord(22, 51), new GeoCoord(22, 55),
            new GeoCoord(24, 56), new GeoCoord(26, 56),
            new GeoCoord(30, 48),
        }));

        zones.Add(MakeCountry("Georgia-Armenia-Azerbaijan", "Asia", new[]
        {
            new GeoCoord(43, 40), new GeoCoord(42, 50),
            new GeoCoord(39, 50), new GeoCoord(39, 44),
            new GeoCoord(40, 44), new GeoCoord(41, 40),
            new GeoCoord(42, 40),
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

        zones.Add(MakeCountry("Papua New Guinea", "Oceania", new[]
        {
            new GeoCoord(-2.5, 141), new GeoCoord(-2, 147),
            new GeoCoord(-4, 152), new GeoCoord(-5.5, 155),
            new GeoCoord(-7, 156), new GeoCoord(-10.5, 152),
            new GeoCoord(-10.5, 147), new GeoCoord(-8, 143),
            new GeoCoord(-5, 141),
        }));

        zones.Add(MakeCountry("Solomon Islands", "Oceania", new[]
        {
            new GeoCoord(-5, 155), new GeoCoord(-5, 162),
            new GeoCoord(-7, 167), new GeoCoord(-12, 168),
            new GeoCoord(-12, 155), new GeoCoord(-8, 155),
        }));

        zones.Add(MakeCountry("Vanuatu-New Caledonia", "Oceania", new[]
        {
            new GeoCoord(-13, 163), new GeoCoord(-13, 170),
            new GeoCoord(-23, 170), new GeoCoord(-23, 163),
        }));

        zones.Add(MakeCountry("Fiji", "Oceania", new[]
        {
            new GeoCoord(-12, 176), new GeoCoord(-12, -178),
            new GeoCoord(-22, -178), new GeoCoord(-22, 176),
        }));

        zones.Add(MakeCountry("Samoa-Tonga", "Oceania", new[]
        {
            new GeoCoord(-13, -178), new GeoCoord(-13, -168),
            new GeoCoord(-23, -168), new GeoCoord(-23, -178),
        }));

        zones.Add(MakeCountry("Micronesia", "Oceania", new[]
        {
            new GeoCoord(-1, 131), new GeoCoord(-1, 172),
            new GeoCoord(20, 172), new GeoCoord(20, 131),
        }));

        zones.Add(MakeCountry("French Polynesia", "Oceania", new[]
        {
            new GeoCoord(-8, -155), new GeoCoord(-8, -134),
            new GeoCoord(-28, -134), new GeoCoord(-28, -155),
        }));

        // Antarctica (5 zones split by longitude)
        zones.Add(MakeCountry("Antarctica (Marie Byrd Land)", "Antarctica", new[]
        {
            new GeoCoord(-70, -180), new GeoCoord(-70, -108),
            new GeoCoord(-90, -108), new GeoCoord(-90, -180),
        }));

        zones.Add(MakeCountry("Antarctica (Peninsula)", "Antarctica", new[]
        {
            new GeoCoord(-70, -108), new GeoCoord(-70, -36),
            new GeoCoord(-90, -36), new GeoCoord(-90, -108),
        }));

        zones.Add(MakeCountry("Antarctica (Queen Maud Land)", "Antarctica", new[]
        {
            new GeoCoord(-70, -36), new GeoCoord(-70, 36),
            new GeoCoord(-90, 36), new GeoCoord(-90, -36),
        }));

        zones.Add(MakeCountry("Antarctica (Enderby Land)", "Antarctica", new[]
        {
            new GeoCoord(-70, 36), new GeoCoord(-70, 108),
            new GeoCoord(-90, 108), new GeoCoord(-90, 36),
        }));

        zones.Add(MakeCountry("Antarctica (Victoria Land)", "Antarctica", new[]
        {
            new GeoCoord(-70, 108), new GeoCoord(-70, 180),
            new GeoCoord(-90, 180), new GeoCoord(-90, 108),
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
