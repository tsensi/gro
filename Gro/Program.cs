using System;
using SDL2;
using Gro.EarthModel;
using Gro.Rendering;

namespace Gro;

public static class Program
{
    public static int Main(string[] args)
    {
        bool headless = Array.Exists(args, a => a == "--headless");

        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            Console.Error.WriteLine($"SDL_Init failed: {SDL.SDL_GetError()}");
            return 1;
        }

        IntPtr window = SDL.SDL_CreateWindow(
            "Gro",
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            1280, 720,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
        );

        if (window == IntPtr.Zero)
        {
            Console.Error.WriteLine($"SDL_CreateWindow failed: {SDL.SDL_GetError()}");
            SDL.SDL_Quit();
            return 1;
        }

        IntPtr renderer = SDL.SDL_CreateRenderer(window, -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        if (renderer == IntPtr.Zero)
        {
            renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_SOFTWARE);
        }

        if (renderer == IntPtr.Zero)
        {
            Console.Error.WriteLine($"SDL_CreateRenderer failed: {SDL.SDL_GetError()}");
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
            return 1;
        }

        var earth = Earth.Create();
        var globe = new GlobeRenderer(earth);

        if (headless)
        {
            SDL.SDL_GetWindowSize(window, out int w, out int h);
            globe.Render(renderer, w, h);
            Console.WriteLine("Globe rendered successfully. Exiting headless mode.");
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
            return 0;
        }

        bool running = true;
        while (running)
        {
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    running = false;
                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN &&
                    e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                    running = false;

                globe.HandleInput(e);
            }

            SDL.SDL_GetWindowSize(window, out int width, out int height);
            globe.Render(renderer, width, height);
        }

        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
        return 0;
    }
}
