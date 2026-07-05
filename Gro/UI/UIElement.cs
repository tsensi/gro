namespace Gro.UI;

public enum LayoutDirection { Vertical, Horizontal }
public enum Alignment { Start, Center, End }

public sealed class UIElement
{
    public string Tag { get; init; } = "";
    public string? Key { get; init; }
    public string? Text { get; init; }
    public UIStyle Style { get; init; } = UIStyle.Default;
    public List<UIElement> Children { get; init; } = new();
    public Action? OnClick { get; init; }
    public Func<UIElement>? Render { get; init; }

    public static UIElement Panel(UIStyle? style = null, string? key = null, params UIElement[] children) =>
        new() { Tag = "panel", Style = style ?? UIStyle.Default, Key = key, Children = children.ToList() };

    public static UIElement Row(UIStyle? style = null, string? key = null, params UIElement[] children) =>
        new() { Tag = "row", Style = style ?? new UIStyle { Direction = LayoutDirection.Horizontal }, Key = key, Children = children.ToList() };

    public static UIElement Label(string text, UIStyle? style = null) =>
        new() { Tag = "label", Text = text, Style = style ?? UIStyle.Default };

    public static UIElement Button(string text, Action onClick, UIStyle? style = null) =>
        new() { Tag = "button", Text = text, OnClick = onClick, Style = style ?? UIStyle.ButtonDefault };

    public static UIElement Component(Func<UIElement> render, string? key = null) =>
        new() { Tag = "component", Render = render, Key = key };
}
