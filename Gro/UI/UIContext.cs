using SDL2;
using Gro.Rendering;

namespace Gro.UI;

public sealed class UIContext
{
    private UINode? _root;
    private int _mouseX;
    private int _mouseY;
    private bool _mouseDown;
    private bool _mouseClicked;

    public bool HandledInput { get; private set; }

    public void Update(Func<UIElement> tree)
    {
        var element = tree();
        _root = Reconcile(element);
    }

    public void HandleInput(SDL.SDL_Event e)
    {
        HandledInput = false;

        if (e.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
        {
            _mouseX = e.motion.x;
            _mouseY = e.motion.y;
        }

        if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN && e.button.button == SDL.SDL_BUTTON_LEFT)
        {
            _mouseX = e.button.x;
            _mouseY = e.button.y;
            _mouseDown = true;
            _mouseClicked = true;
        }

        if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP && e.button.button == SDL.SDL_BUTTON_LEFT)
        {
            _mouseDown = false;
        }

        if (_root != null)
        {
            UpdateHitTest(_root);
            if (_mouseClicked)
            {
                ProcessClick(_root);
            }
        }

        _mouseClicked = false;
    }

    public void Layout(int windowWidth, int windowHeight)
    {
        if (_root == null) return;
        LayoutNode(_root, 0, 0, windowWidth, windowHeight);
        ApplyAnchors(_root, windowWidth, windowHeight);
    }

    public void Render(IntPtr renderer)
    {
        if (_root == null) return;
        RenderNode(renderer, _root);
    }

    private UINode Reconcile(UIElement element)
    {
        var resolved = element;
        if (element.Tag == "component" && element.Render != null)
        {
            resolved = element.Render();
        }

        var node = new UINode { Element = resolved };
        foreach (var child in resolved.Children)
        {
            node.Children.Add(Reconcile(child));
        }
        return node;
    }

    private void LayoutNode(UINode node, int x, int y, int availW, int availH)
    {
        var style = node.Element.Style;
        int w = style.Width ?? availW;
        int h = style.Height ?? availH;

        if (node.Element.Tag == "label" || node.Element.Tag == "button")
        {
            int textW = (node.Element.Text?.Length ?? 0) * CharWidth(style.FontSize);
            int textH = style.FontSize + 2;
            if (style.Width == null) w = textW + style.Padding * 2;
            if (style.Height == null) h = textH + style.Padding * 2;
        }

        if (node.Children.Count > 0 && style.Width == null && style.Height == null
            && node.Element.Tag != "label" && node.Element.Tag != "button")
        {
            h = MeasureHeight(node, availW, availH);
            w = MeasureWidth(node, availW);
        }

        node.Bounds = new Rect(x, y, w, h);

        int offset = style.Padding;
        int innerW = w - style.Padding * 2;
        int innerH = h - style.Padding * 2;

        foreach (var child in node.Children)
        {
            int childX, childY;
            if (style.Direction == LayoutDirection.Vertical)
            {
                childX = x + style.Padding;
                childY = y + offset;
                int childAvailH = innerH;
                LayoutNode(child, childX, childY, innerW, childAvailH);
                offset += child.Bounds.H + style.Gap;
            }
            else
            {
                childX = x + offset;
                childY = y + style.Padding;
                int childAvailW = innerW;
                LayoutNode(child, childX, childY, childAvailW, innerH);
                offset += child.Bounds.W + style.Gap;
            }
        }
    }

    private void ApplyAnchors(UINode node, int windowWidth, int windowHeight)
    {
        var style = node.Element.Style;
        if (style.Anchor == Anchor.TopRight)
        {
            int x = windowWidth - node.Bounds.W + style.OffsetX;
            int y = style.OffsetY;
            ShiftNode(node, x - node.Bounds.X, y - node.Bounds.Y);
        }
        else if (style.Anchor == Anchor.Center)
        {
            int x = (windowWidth - node.Bounds.W) / 2 + style.OffsetX;
            int y = (windowHeight - node.Bounds.H) / 2 + style.OffsetY;
            ShiftNode(node, x - node.Bounds.X, y - node.Bounds.Y);
        }
        else if (style.OffsetX != 0 || style.OffsetY != 0)
        {
            ShiftNode(node, style.OffsetX, style.OffsetY);
        }

        foreach (var child in node.Children)
            ApplyAnchors(child, windowWidth, windowHeight);
    }

    private void ShiftNode(UINode node, int dx, int dy)
    {
        node.Bounds = new Rect(node.Bounds.X + dx, node.Bounds.Y + dy, node.Bounds.W, node.Bounds.H);
        foreach (var child in node.Children)
            ShiftNode(child, dx, dy);
    }

    private int MeasureHeight(UINode node, int availW, int availH)
    {
        var style = node.Element.Style;
        if (style.Height.HasValue) return style.Height.Value;
        if (style.Direction == LayoutDirection.Vertical)
        {
            int total = style.Padding * 2;
            for (int i = 0; i < node.Children.Count; i++)
            {
                total += MeasureChildHeight(node.Children[i], availW, availH);
                if (i < node.Children.Count - 1) total += style.Gap;
            }
            return total;
        }
        else
        {
            int max = 0;
            foreach (var child in node.Children)
            {
                int ch = MeasureChildHeight(child, availW, availH);
                if (ch > max) max = ch;
            }
            return max + style.Padding * 2;
        }
    }

    private int MeasureWidth(UINode node, int availW)
    {
        var style = node.Element.Style;
        if (style.Width.HasValue) return style.Width.Value;
        if (style.Direction == LayoutDirection.Horizontal)
        {
            int total = style.Padding * 2;
            for (int i = 0; i < node.Children.Count; i++)
            {
                total += MeasureChildWidth(node.Children[i], availW);
                if (i < node.Children.Count - 1) total += style.Gap;
            }
            return total;
        }
        else
        {
            int max = 0;
            foreach (var child in node.Children)
            {
                int cw = MeasureChildWidth(child, availW);
                if (cw > max) max = cw;
            }
            return max + style.Padding * 2;
        }
    }

    private int MeasureChildHeight(UINode child, int availW, int availH)
    {
        var style = child.Element.Style;
        if (style.Height.HasValue) return style.Height.Value;
        if (child.Element.Tag == "label" || child.Element.Tag == "button")
            return style.FontSize + 2 + style.Padding * 2;
        return MeasureHeight(child, availW, availH);
    }

    private int MeasureChildWidth(UINode child, int availW)
    {
        var style = child.Element.Style;
        if (style.Width.HasValue) return style.Width.Value;
        if (child.Element.Tag == "label" || child.Element.Tag == "button")
        {
            int textW = (child.Element.Text?.Length ?? 0) * CharWidth(style.FontSize);
            return textW + style.Padding * 2;
        }
        return MeasureWidth(child, availW);
    }

    private static int CharWidth(int fontSize) => fontSize * 6 / 10 + 1;

    private void UpdateHitTest(UINode node)
    {
        node.Hovered = node.Bounds.Contains(_mouseX, _mouseY);
        node.Pressed = node.Hovered && _mouseDown;
        foreach (var child in node.Children)
            UpdateHitTest(child);
    }

    private bool ProcessClick(UINode node)
    {
        foreach (var child in node.Children)
        {
            if (ProcessClick(child))
                return true;
        }

        if (node.Hovered && node.Element.OnClick != null)
        {
            node.Element.OnClick();
            HandledInput = true;
            return true;
        }
        return false;
    }

    private void RenderNode(IntPtr renderer, UINode node)
    {
        var style = node.Element.Style;
        var bounds = node.Bounds;

        if (style.BackgroundColor.A > 0)
        {
            var bg = style.BackgroundColor;
            if (node.Element.Tag == "button" && node.Hovered)
                bg = Color.FromRgba((byte)Math.Min(255, bg.R + 30), (byte)Math.Min(255, bg.G + 30), (byte)Math.Min(255, bg.B + 30), bg.A);

            SDL.SDL_SetRenderDrawBlendMode(renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetRenderDrawColor(renderer, bg.R, bg.G, bg.B, bg.A);
            var rect = new SDL.SDL_Rect { x = bounds.X, y = bounds.Y, w = bounds.W, h = bounds.H };
            SDL.SDL_RenderFillRect(renderer, ref rect);
        }

        if (style.BorderWidth > 0 && style.BorderColor.A > 0)
        {
            var bc = style.BorderColor;
            SDL.SDL_SetRenderDrawColor(renderer, bc.R, bc.G, bc.B, bc.A);
            for (int i = 0; i < style.BorderWidth; i++)
            {
                var rect = new SDL.SDL_Rect { x = bounds.X + i, y = bounds.Y + i, w = bounds.W - i * 2, h = bounds.H - i * 2 };
                SDL.SDL_RenderDrawRect(renderer, ref rect);
            }
        }

        if (node.Element.Text != null)
        {
            DrawText(renderer, node.Element.Text, bounds.X + style.Padding, bounds.Y + style.Padding, style.TextColor, style.FontSize);
        }

        foreach (var child in node.Children)
            RenderNode(renderer, child);
    }

    private static void DrawText(IntPtr renderer, string text, int x, int y, Color color, int fontSize)
    {
        SDL.SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        int charW = CharWidth(fontSize);
        int charH = fontSize;

        foreach (char c in text)
        {
            if (c >= 32 && c < 127)
            {
                DrawGlyph(renderer, c, x, y, charW, charH);
            }
            x += charW;
        }
    }

    private static void DrawGlyph(IntPtr renderer, char c, int x, int y, int w, int h)
    {
        var bitmap = BitmapFont.GetGlyph(c);
        if (bitmap == null) return;

        int rows = bitmap.Length;
        int cols = bitmap[0].Length;
        float scaleY = (float)h / rows;
        float scaleX = (float)w / cols;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (bitmap[row][col] == 1)
                {
                    int px = x + (int)(col * scaleX);
                    int py = y + (int)(row * scaleY);
                    int pw = Math.Max(1, (int)scaleX);
                    int ph = Math.Max(1, (int)scaleY);
                    var rect = new SDL.SDL_Rect { x = px, y = py, w = pw, h = ph };
                    SDL.SDL_RenderFillRect(renderer, ref rect);
                }
            }
        }
    }
}
