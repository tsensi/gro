namespace Gro.Rendering;

public readonly record struct Color(byte R, byte G, byte B, byte A)
{
    public static Color FromRgb(byte r, byte g, byte b) => new(r, g, b, 255);
    public static Color FromRgba(byte r, byte g, byte b, byte a) => new(r, g, b, a);
}

public sealed class ZoneStyle
{
    public Color OutlineColor { get; init; } = Color.FromRgb(100, 180, 100);
    public int OutlineWidth { get; init; } = 1;
    public Color FillColor { get; init; } = Color.FromRgba(0, 0, 0, 0);
    public bool FillEnabled { get; init; }
}
