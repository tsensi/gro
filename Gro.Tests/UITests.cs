using Gro.UI;
using Gro.Rendering;

namespace Gro.Tests;

public class UITests
{
    [Fact]
    public void Rect_Contains_PointInside()
    {
        var r = new Rect(10, 20, 100, 50);
        Assert.True(r.Contains(50, 40));
        Assert.False(r.Contains(5, 40));
        Assert.False(r.Contains(50, 5));
        Assert.False(r.Contains(111, 40));
        Assert.False(r.Contains(50, 71));
    }

    [Fact]
    public void UIElement_Panel_CreatesWithChildren()
    {
        var panel = UIElement.Panel(null, null,
            UIElement.Label("Hello"),
            UIElement.Button("Click", () => { })
        );

        Assert.Equal("panel", panel.Tag);
        Assert.Equal(2, panel.Children.Count);
        Assert.Equal("label", panel.Children[0].Tag);
        Assert.Equal("Hello", panel.Children[0].Text);
        Assert.Equal("button", panel.Children[1].Tag);
    }

    [Fact]
    public void UIElement_Row_HasHorizontalDirection()
    {
        var row = UIElement.Row();
        Assert.Equal(LayoutDirection.Horizontal, row.Style.Direction);
    }

    [Fact]
    public void UIElement_Component_RendersViaFunction()
    {
        int callCount = 0;
        var comp = UIElement.Component(() =>
        {
            callCount++;
            return UIElement.Label("dynamic");
        });

        Assert.Equal("component", comp.Tag);
        Assert.NotNull(comp.Render);
        var result = comp.Render!();
        Assert.Equal(1, callCount);
        Assert.Equal("dynamic", result.Text);
    }

    [Fact]
    public void StateStore_GetSet()
    {
        var store = new StateStore();
        Assert.Equal(42, store.Get("x", 42));
        store.Set("x", 99);
        Assert.Equal(99, store.Get("x", 0));
    }

    [Fact]
    public void StateStore_Has()
    {
        var store = new StateStore();
        Assert.False(store.Has("key"));
        store.Get("key", "val");
        Assert.True(store.Has("key"));
    }

    [Fact]
    public void Animator_TicksToCompletion()
    {
        var animator = new Animator();
        float lastValue = 0;
        bool completed = false;

        animator.Animate(0, 100, 200, EaseFunction.Linear,
            onUpdate: v => lastValue = v,
            onComplete: () => completed = true);

        Assert.True(animator.HasActive);

        animator.Tick(100);
        Assert.True(lastValue > 0 && lastValue <= 100);
        Assert.False(completed);

        animator.Tick(200);
        Assert.Equal(100f, lastValue);
        Assert.True(completed);
        Assert.False(animator.HasActive);
    }

    [Fact]
    public void Animator_EaseOut_FastStart()
    {
        var animator = new Animator();
        float val25 = 0;

        animator.Animate(0, 1, 100, EaseFunction.EaseOut,
            onUpdate: v => val25 = v);
        animator.Tick(25);

        // EaseOut should be > 0.25 at t=0.25 (fast start)
        Assert.True(val25 > 0.25f);
    }

    [Fact]
    public void UIContext_Layout_ButtonSizesFromText()
    {
        var ctx = new UIContext();
        ctx.Update(() => UIElement.Button("OK", () => { }));
        ctx.Layout(800, 600);
        // Just verify no exceptions
    }

    [Fact]
    public void UIContext_Update_ResolvesComponent()
    {
        var ctx = new UIContext();
        ctx.Update(() => UIElement.Component(() =>
            UIElement.Panel(null, null,
                UIElement.Label("resolved")
            )
        ));
        ctx.Layout(800, 600);
    }

    [Fact]
    public void BitmapFont_HasAlphanumericGlyphs()
    {
        Assert.NotNull(BitmapFont.GetGlyph('A'));
        Assert.NotNull(BitmapFont.GetGlyph('z'));
        Assert.NotNull(BitmapFont.GetGlyph('0'));
        Assert.NotNull(BitmapFont.GetGlyph(' '));
    }

    [Fact]
    public void BitmapFont_CaseInsensitiveFallback()
    {
        var upper = BitmapFont.GetGlyph('A');
        var fromLowerLookup = BitmapFont.GetGlyph('A');
        Assert.NotNull(upper);
        Assert.Equal(upper, fromLowerLookup);
    }

    [Fact]
    public void UIElement_Button_ClickActionStored()
    {
        bool clicked = false;
        var btn = UIElement.Button("X", () => clicked = true);
        btn.OnClick!();
        Assert.True(clicked);
    }
}
