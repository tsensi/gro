namespace Gro.UI;

public sealed class UINode
{
    public UIElement Element { get; set; } = null!;
    public Rect Bounds { get; set; }
    public List<UINode> Children { get; } = new();
    public bool Hovered { get; set; }
    public bool Pressed { get; set; }
}
