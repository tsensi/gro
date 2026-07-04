using Gro.EarthModel;

namespace Gro.Rendering;

public static class ZoneStyleProvider
{
    public static void ApplyDefaults(IReadOnlyList<Zone> zones)
    {
        for (int i = 0; i < zones.Count; i++)
        {
            zones[i].Style = CreateDefault(zones[i], i);
        }
    }

    private static ZoneStyle CreateDefault(Zone zone, int index)
    {
        int hash = zone.Name.GetHashCode();
        byte variation = (byte)((hash & 0x7F) % 40);

        return zone.Type switch
        {
            ZoneType.Continent => new ZoneStyle
            {
                OutlineColor = Color.FromRgb((byte)(50 + variation / 2), (byte)(90 + variation), (byte)(50 + variation / 2)),
                OutlineWidth = 2,
                FillColor = Color.FromRgba((byte)(30 + variation / 2), (byte)(60 + variation / 2), (byte)(30 + variation / 2), 40),
                FillEnabled = true,
            },
            ZoneType.Country => new ZoneStyle
            {
                OutlineColor = Color.FromRgb((byte)(80 + variation), (byte)(160 + variation / 2), (byte)(80 + variation)),
                OutlineWidth = 1,
                FillColor = Color.FromRgba((byte)(40 + variation), (byte)(80 + variation), (byte)(40 + variation / 2), 30),
                FillEnabled = true,
            },
            ZoneType.OceanBasin => new ZoneStyle
            {
                OutlineColor = Color.FromRgb((byte)(30 + variation / 2), (byte)(60 + variation), (byte)(120 + variation)),
                OutlineWidth = 2,
                FillColor = Color.FromRgba((byte)(20 + variation / 3), (byte)(40 + variation / 2), (byte)(80 + variation), 35),
                FillEnabled = true,
            },
            ZoneType.OceanZone => new ZoneStyle
            {
                OutlineColor = Color.FromRgb((byte)(40 + variation), (byte)(80 + variation), (byte)(140 + variation / 2)),
                OutlineWidth = 1,
                FillColor = Color.FromRgba((byte)(25 + variation / 2), (byte)(50 + variation / 2), (byte)(100 + variation), 25),
                FillEnabled = true,
            },
            _ => new ZoneStyle(),
        };
    }
}
