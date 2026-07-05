using Gro.Rendering;

namespace Gro.UI;

public sealed class UIStyle
{
    public LayoutDirection Direction { get; init; } = LayoutDirection.Vertical;
    public Alignment AlignItems { get; init; } = Alignment.Start;
    public int Padding { get; init; }
    public int Gap { get; init; }
    public int? Width { get; init; }
    public int? Height { get; init; }
    public Color BackgroundColor { get; init; } = Color.FromRgba(0, 0, 0, 0);
    public Color TextColor { get; init; } = Color.FromRgb(220, 220, 220);
    public Color BorderColor { get; init; } = Color.FromRgba(0, 0, 0, 0);
    public int BorderWidth { get; init; }
    public int FontSize { get; init; } = 14;

    public static readonly UIStyle Default = new();

    public static readonly UIStyle ButtonDefault = new()
    {
        Padding = 6,
        BackgroundColor = Color.FromRgba(60, 80, 120, 220),
        TextColor = Color.FromRgb(240, 240, 240),
        BorderColor = Color.FromRgba(100, 140, 200, 200),
        BorderWidth = 1,
    };
}
