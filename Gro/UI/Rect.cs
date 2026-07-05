namespace Gro.UI;

public readonly record struct Rect(int X, int Y, int W, int H)
{
    public bool Contains(int px, int py) =>
        px >= X && px < X + W && py >= Y && py < Y + H;
}
