using System;
using System.Collections.Generic;
using SDL2;

namespace Gro.Rendering;

public enum DotShape
{
    SmallDot,
    Circle,
    Square,
    Diamond,
    Triangle,
    Star,
    Pentagon,
    Hexagon,
    Cross,
    Ring,
    Octagon,
}

public static class DotCounter
{
    private static readonly DotShape[] Shapes =
    {
        DotShape.SmallDot,   // 10^0 = 1
        DotShape.Circle,     // 10^1 = 10
        DotShape.Square,     // 10^2 = 100
        DotShape.Diamond,    // 10^3 = 1,000
        DotShape.Triangle,   // 10^4 = 10,000
        DotShape.Star,       // 10^5 = 100,000
        DotShape.Pentagon,   // 10^6 = 1,000,000
        DotShape.Hexagon,    // 10^7 = 10,000,000
        DotShape.Cross,      // 10^8 = 100,000,000
        DotShape.Ring,       // 10^9 = 1,000,000,000
        DotShape.Octagon,    // 10^10 = 10,000,000,000
    };

    public const int GridSize = 9;

    public static List<DotShape> Decompose(double value)
    {
        var dots = new List<DotShape>();
        if (value <= 0) return dots;

        if (value < 1)
        {
            dots.Add(DotShape.SmallDot);
            return dots;
        }

        long remaining = (long)Math.Min(value, long.MaxValue / 2);

        for (int mag = Shapes.Length - 1; mag >= 0; mag--)
        {
            long divisor = (long)Math.Pow(10, mag);
            int count = (int)(remaining / divisor);
            if (count > 9) count = 9;
            for (int i = 0; i < count; i++)
                dots.Add(Shapes[mag]);
            remaining -= count * divisor;
        }

        if (dots.Count > GridSize)
            dots.RemoveRange(GridSize, dots.Count - GridSize);

        return dots;
    }

    public static void DrawDot(IntPtr renderer, DotShape shape, int cx, int cy, int size)
    {
        switch (shape)
        {
            case DotShape.SmallDot:
                DrawFilledCircle(renderer, cx, cy, Math.Max(1, size / 3));
                break;
            case DotShape.Circle:
                DrawCircleOutline(renderer, cx, cy, size / 2);
                break;
            case DotShape.Square:
                DrawSquare(renderer, cx, cy, size);
                break;
            case DotShape.Diamond:
                DrawDiamond(renderer, cx, cy, size);
                break;
            case DotShape.Triangle:
                DrawTriangle(renderer, cx, cy, size);
                break;
            case DotShape.Star:
                DrawStar(renderer, cx, cy, size);
                break;
            case DotShape.Pentagon:
                DrawPolygonOutline(renderer, cx, cy, size / 2, 5);
                break;
            case DotShape.Hexagon:
                DrawPolygonOutline(renderer, cx, cy, size / 2, 6);
                break;
            case DotShape.Cross:
                DrawCross(renderer, cx, cy, size);
                break;
            case DotShape.Ring:
                DrawCircleOutline(renderer, cx, cy, size / 2);
                DrawCircleOutline(renderer, cx, cy, size / 2 - 2);
                break;
            case DotShape.Octagon:
                DrawPolygonOutline(renderer, cx, cy, size / 2, 8);
                break;
        }
    }

    private static void DrawFilledCircle(IntPtr renderer, int cx, int cy, int r)
    {
        for (int dy = -r; dy <= r; dy++)
        {
            int dx = (int)Math.Sqrt(r * r - dy * dy);
            SDL.SDL_RenderDrawLine(renderer, cx - dx, cy + dy, cx + dx, cy + dy);
        }
    }

    private static void DrawCircleOutline(IntPtr renderer, int cx, int cy, int r)
    {
        int x = r, y = 0;
        int d = 1 - r;
        while (x >= y)
        {
            SDL.SDL_RenderDrawPoint(renderer, cx + x, cy + y);
            SDL.SDL_RenderDrawPoint(renderer, cx - x, cy + y);
            SDL.SDL_RenderDrawPoint(renderer, cx + x, cy - y);
            SDL.SDL_RenderDrawPoint(renderer, cx - x, cy - y);
            SDL.SDL_RenderDrawPoint(renderer, cx + y, cy + x);
            SDL.SDL_RenderDrawPoint(renderer, cx - y, cy + x);
            SDL.SDL_RenderDrawPoint(renderer, cx + y, cy - x);
            SDL.SDL_RenderDrawPoint(renderer, cx - y, cy - x);
            y++;
            if (d < 0)
                d += 2 * y + 1;
            else
            {
                x--;
                d += 2 * (y - x) + 1;
            }
        }
    }

    private static void DrawSquare(IntPtr renderer, int cx, int cy, int size)
    {
        int half = size / 2;
        var rect = new SDL.SDL_Rect { x = cx - half, y = cy - half, w = size, h = size };
        SDL.SDL_RenderDrawRect(renderer, ref rect);
    }

    private static void DrawDiamond(IntPtr renderer, int cx, int cy, int size)
    {
        int half = size / 2;
        SDL.SDL_RenderDrawLine(renderer, cx, cy - half, cx + half, cy);
        SDL.SDL_RenderDrawLine(renderer, cx + half, cy, cx, cy + half);
        SDL.SDL_RenderDrawLine(renderer, cx, cy + half, cx - half, cy);
        SDL.SDL_RenderDrawLine(renderer, cx - half, cy, cx, cy - half);
    }

    private static void DrawTriangle(IntPtr renderer, int cx, int cy, int size)
    {
        int half = size / 2;
        int top = cy - half;
        int bot = cy + half;
        SDL.SDL_RenderDrawLine(renderer, cx, top, cx + half, bot);
        SDL.SDL_RenderDrawLine(renderer, cx + half, bot, cx - half, bot);
        SDL.SDL_RenderDrawLine(renderer, cx - half, bot, cx, top);
    }

    private static void DrawStar(IntPtr renderer, int cx, int cy, int size)
    {
        int half = size / 2;
        SDL.SDL_RenderDrawLine(renderer, cx, cy - half, cx, cy + half);
        SDL.SDL_RenderDrawLine(renderer, cx - half, cy, cx + half, cy);
        int d = (int)(half * 0.7);
        SDL.SDL_RenderDrawLine(renderer, cx - d, cy - d, cx + d, cy + d);
        SDL.SDL_RenderDrawLine(renderer, cx + d, cy - d, cx - d, cy + d);
    }

    private static void DrawPolygonOutline(IntPtr renderer, int cx, int cy, int r, int sides)
    {
        double angleStep = 2 * Math.PI / sides;
        double startAngle = -Math.PI / 2;
        int prevX = cx + (int)(r * Math.Cos(startAngle));
        int prevY = cy + (int)(r * Math.Sin(startAngle));
        for (int i = 1; i <= sides; i++)
        {
            double angle = startAngle + i * angleStep;
            int nx = cx + (int)(r * Math.Cos(angle));
            int ny = cy + (int)(r * Math.Sin(angle));
            SDL.SDL_RenderDrawLine(renderer, prevX, prevY, nx, ny);
            prevX = nx;
            prevY = ny;
        }
    }

    private static void DrawCross(IntPtr renderer, int cx, int cy, int size)
    {
        int half = size / 2;
        int arm = size / 4;
        SDL.SDL_RenderDrawLine(renderer, cx - arm, cy - half, cx + arm, cy - half);
        SDL.SDL_RenderDrawLine(renderer, cx + arm, cy - half, cx + arm, cy - arm);
        SDL.SDL_RenderDrawLine(renderer, cx + arm, cy - arm, cx + half, cy - arm);
        SDL.SDL_RenderDrawLine(renderer, cx + half, cy - arm, cx + half, cy + arm);
        SDL.SDL_RenderDrawLine(renderer, cx + half, cy + arm, cx + arm, cy + arm);
        SDL.SDL_RenderDrawLine(renderer, cx + arm, cy + arm, cx + arm, cy + half);
        SDL.SDL_RenderDrawLine(renderer, cx + arm, cy + half, cx - arm, cy + half);
        SDL.SDL_RenderDrawLine(renderer, cx - arm, cy + half, cx - arm, cy + arm);
        SDL.SDL_RenderDrawLine(renderer, cx - arm, cy + arm, cx - half, cy + arm);
        SDL.SDL_RenderDrawLine(renderer, cx - half, cy + arm, cx - half, cy - arm);
        SDL.SDL_RenderDrawLine(renderer, cx - half, cy - arm, cx - arm, cy - arm);
        SDL.SDL_RenderDrawLine(renderer, cx - arm, cy - arm, cx - arm, cy - half);
    }
}
