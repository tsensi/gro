using System;
using SDL2;

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
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN
        );

        if (window == IntPtr.Zero)
        {
            Console.Error.WriteLine($"SDL_CreateWindow failed: {SDL.SDL_GetError()}");
            SDL.SDL_Quit();
            return 1;
        }

        if (headless)
        {
            Console.WriteLine("SDL window created successfully. Exiting headless mode.");
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
            }

            SDL.SDL_Delay(16);
        }

        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
        return 0;
    }
}
