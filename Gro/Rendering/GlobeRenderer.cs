using System;
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
            switch (zone.Type)
            {
                case ZoneType.Continent:
                    SDL.SDL_SetRenderDrawColor(renderer, 60, 100, 60, 255);
                    break;
                case ZoneType.Country:
                    SDL.SDL_SetRenderDrawColor(renderer, 100, 180, 100, 255);
                    break;
                case ZoneType.OceanBasin:
                    SDL.SDL_SetRenderDrawColor(renderer, 40, 80, 140, 255);
                    break;
                case ZoneType.OceanZone:
                    SDL.SDL_SetRenderDrawColor(renderer, 50, 100, 170, 255);
                    break;
            }

            DrawPolygon(renderer, zone.Boundary);
        }
    }

    private void DrawPolygon(IntPtr renderer, GeoCoord[] boundary)
    {
        if (boundary.Length < 2) return;

        for (int i = 0; i < boundary.Length; i++)
        {
            int j = (i + 1) % boundary.Length;
            var (x1, y1, v1) = Project(boundary[i].Lat, boundary[i].Lon);
            var (x2, y2, v2) = Project(boundary[j].Lat, boundary[j].Lon);

            if (v1 && v2)
                SDL.SDL_RenderDrawLine(renderer, x1, y1, x2, y2);
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
}
