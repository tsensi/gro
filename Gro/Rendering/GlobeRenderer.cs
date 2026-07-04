using System;
using System.Collections.Generic;
using SDL2;
using Gro.EarthModel;

namespace Gro.Rendering;

public sealed class GlobeRenderer
{
    private readonly Earth _earth;
    private double _rotLon;
    private double _rotLat = 20.0;
    private int _centerX;
    private int _centerY;
    private int _radius;

    public GlobeRenderer(Earth earth)
    {
        _earth = earth;
        ZoneStyleProvider.ApplyDefaults(_earth.Zones);
    }

    public void HandleInput(SDL.SDL_Event e)
    {
        if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
        {
            switch (e.key.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_LEFT:
                    _rotLon -= 5;
                    break;
                case SDL.SDL_Keycode.SDLK_RIGHT:
                    _rotLon += 5;
                    break;
                case SDL.SDL_Keycode.SDLK_UP:
                    _rotLat = Math.Min(90, _rotLat + 5);
                    break;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    _rotLat = Math.Max(-90, _rotLat - 5);
                    break;
            }
        }

        if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN && e.button.button == SDL.SDL_BUTTON_LEFT)
        {
            var geo = Unproject(e.button.x, e.button.y);
            if (geo.HasValue)
            {
                var zone = _earth.ZoneAt(geo.Value);
                if (zone != null)
                    Console.WriteLine($"Selected zone: {zone.Name} ({zone.Type})");
                else
                    Console.WriteLine($"Clicked at ({geo.Value.Lat:F1}, {geo.Value.Lon:F1}) - no zone found");
            }
        }
    }

    public void Render(IntPtr renderer, int windowWidth, int windowHeight)
    {
        _centerX = windowWidth / 2;
        _centerY = windowHeight / 2;
        _radius = Math.Min(windowWidth, windowHeight) / 2 - 40;

        SDL.SDL_SetRenderDrawColor(renderer, 10, 10, 30, 255);
        SDL.SDL_RenderClear(renderer);

        DrawGlobeOutline(renderer);
        DrawGraticule(renderer);
        DrawZones(renderer);

        SDL.SDL_RenderPresent(renderer);
    }

    private void DrawGlobeOutline(IntPtr renderer)
    {
        SDL.SDL_SetRenderDrawColor(renderer, 40, 60, 100, 255);
        const int segments = 64;
        for (int i = 0; i < segments; i++)
        {
            double a1 = 2 * Math.PI * i / segments;
            double a2 = 2 * Math.PI * (i + 1) / segments;
            SDL.SDL_RenderDrawLine(renderer,
                _centerX + (int)(Math.Cos(a1) * _radius),
                _centerY + (int)(Math.Sin(a1) * _radius),
                _centerX + (int)(Math.Cos(a2) * _radius),
                _centerY + (int)(Math.Sin(a2) * _radius));
        }
    }

    private void DrawGraticule(IntPtr renderer)
    {
        SDL.SDL_SetRenderDrawColor(renderer, 30, 45, 70, 255);

        for (int lat = -60; lat <= 60; lat += 30)
        {
            int prevX = 0, prevY = 0;
            bool prevVisible = false;
            for (int lon = 0; lon <= 360; lon += 5)
            {
                var (px, py, visible) = Project(lat, lon - 180);
                if (visible && prevVisible)
                    SDL.SDL_RenderDrawLine(renderer, prevX, prevY, px, py);
                prevX = px;
                prevY = py;
                prevVisible = visible;
            }
        }

        for (int lon = -180; lon < 180; lon += 30)
        {
            int prevX = 0, prevY = 0;
            bool prevVisible = false;
            for (int lat = -90; lat <= 90; lat += 5)
            {
                var (px, py, visible) = Project(lat, lon);
                if (visible && prevVisible)
                    SDL.SDL_RenderDrawLine(renderer, prevX, prevY, px, py);
                prevX = px;
                prevY = py;
                prevVisible = visible;
            }
        }
    }

    private void DrawZones(IntPtr renderer)
    {
        foreach (var zone in _earth.Zones)
        {
            var style = zone.Style;
            var projected = ProjectBoundary(zone.Boundary);

            if (style.FillEnabled && projected.Count >= 3)
            {
                SDL.SDL_SetRenderDrawBlendMode(renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
                var fill = style.FillColor;
                SDL.SDL_SetRenderDrawColor(renderer, fill.R, fill.G, fill.B, fill.A);
                FillProjectedPolygon(renderer, projected);
            }

            var outline = style.OutlineColor;
            SDL.SDL_SetRenderDrawColor(renderer, outline.R, outline.G, outline.B, outline.A);
            DrawOutline(renderer, zone.Boundary, style.OutlineWidth);
        }
    }

    private List<(int x, int y)> ProjectBoundary(GeoCoord[] boundary)
    {
        var points = new List<(int x, int y)>();
        foreach (var coord in boundary)
        {
            var (px, py, visible) = Project(coord.Lat, coord.Lon);
            if (visible)
                points.Add((px, py));
        }
        return points;
    }

    private void FillProjectedPolygon(IntPtr renderer, List<(int x, int y)> points)
    {
        if (points.Count < 3) return;

        int minY = int.MaxValue, maxY = int.MinValue;
        foreach (var (_, y) in points)
        {
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
        }

        minY = Math.Max(minY, _centerY - _radius);
        maxY = Math.Min(maxY, _centerY + _radius);

        var intersections = new List<int>();
        for (int y = minY; y <= maxY; y++)
        {
            intersections.Clear();
            int n = points.Count;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                int yi = points[i].y, yj = points[j].y;
                if ((yi <= y && yj > y) || (yj <= y && yi > y))
                {
                    int xi = points[i].x, xj = points[j].x;
                    int x = xi + (y - yi) * (xj - xi) / (yj - yi);
                    intersections.Add(x);
                }
            }
            intersections.Sort();
            for (int i = 0; i + 1 < intersections.Count; i += 2)
            {
                SDL.SDL_RenderDrawLine(renderer, intersections[i], y, intersections[i + 1], y);
            }
        }
    }

    private void DrawOutline(IntPtr renderer, GeoCoord[] boundary, int width)
    {
        if (boundary.Length < 2) return;

        for (int i = 0; i < boundary.Length; i++)
        {
            int j = (i + 1) % boundary.Length;
            var (x1, y1, v1) = Project(boundary[i].Lat, boundary[i].Lon);
            var (x2, y2, v2) = Project(boundary[j].Lat, boundary[j].Lon);

            if (v1 && v2)
            {
                SDL.SDL_RenderDrawLine(renderer, x1, y1, x2, y2);
                for (int w = 1; w < width; w++)
                {
                    SDL.SDL_RenderDrawLine(renderer, x1, y1 + w, x2, y2 + w);
                    SDL.SDL_RenderDrawLine(renderer, x1, y1 - w, x2, y2 - w);
                }
            }
        }
    }

    private (int x, int y, bool visible) Project(double lat, double lon)
    {
        double latR = lat * Math.PI / 180.0;
        double lonR = (lon - _rotLon) * Math.PI / 180.0;
        double camLatR = _rotLat * Math.PI / 180.0;

        double x = Math.Cos(latR) * Math.Sin(lonR);
        double y = Math.Sin(latR) * Math.Cos(camLatR) - Math.Cos(latR) * Math.Sin(camLatR) * Math.Cos(lonR);
        double z = Math.Sin(latR) * Math.Sin(camLatR) + Math.Cos(latR) * Math.Cos(camLatR) * Math.Cos(lonR);

        bool visible = z > 0;
        int px = _centerX + (int)(x * _radius);
        int py = _centerY - (int)(y * _radius);
        return (px, py, visible);
    }

    private GeoCoord? Unproject(int screenX, int screenY)
    {
        double nx = (screenX - _centerX) / (double)_radius;
        double ny = -((screenY - _centerY) / (double)_radius);

        double r2 = nx * nx + ny * ny;
        if (r2 > 1.0)
            return null;

        double nz = Math.Sqrt(1.0 - r2);

        double camLatR = _rotLat * Math.PI / 180.0;
        double sinCam = Math.Sin(camLatR);
        double cosCam = Math.Cos(camLatR);

        double xRot = nx;
        double yRot = ny * cosCam + nz * sinCam;
        double zRot = -ny * sinCam + nz * cosCam;

        double lat = Math.Asin(Math.Clamp(yRot, -1.0, 1.0)) * 180.0 / Math.PI;
        double lon = Math.Atan2(xRot, zRot) * 180.0 / Math.PI + _rotLon;

        if (lon > 180) lon -= 360;
        if (lon < -180) lon += 360;

        return new GeoCoord(lat, lon);
    }
}
