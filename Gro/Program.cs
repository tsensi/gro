using System;
using SDL2;
using Gro.EarthModel;
using Gro.Rendering;
using Gro.Simulation;
using Gro.UI;

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
        var ui = new UIContext();
        var animator = new Animator();
        var state = new StateStore();
        var sim = new SimLoop();
        uint lastTick = SDL.SDL_GetTicks();

        globe.ZoneSelected += zone =>
        {
            if (zone != null)
            {
                state.Set("selectedZone", zone);
                state.Set("panelDismissing", false);
                animator.Animate(0f, 1f, 250f, EaseFunction.EaseOut,
                    v => state.Set("panelAnim", v));
            }
        };

        if (headless)
        {
            SDL.SDL_GetWindowSize(window, out int w, out int h);
            ui.Update(() => BuildUI(state, animator));
            ui.Layout(w, h);
            globe.Render(renderer, w, h, present: false);
            ui.Render(renderer);
            SDL.SDL_RenderPresent(renderer);
            Console.WriteLine("Globe rendered successfully. Exiting headless mode.");
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
            return 0;
        }

        bool running = true;
        while (running)
        {
            uint now = SDL.SDL_GetTicks();
            float deltaMs = now - lastTick;
            lastTick = now;
            animator.Tick(deltaMs);

            double deltaDays = deltaMs / 1000.0 / 86400.0;
            sim.Tick(deltaDays);

            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    running = false;
                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN &&
                    e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                    running = false;

                ui.HandleInput(e);
                if (!ui.HandledInput)
                    globe.HandleInput(e);
            }

            SDL.SDL_GetWindowSize(window, out int width, out int height);

            ui.Update(() => BuildUI(state, animator));
            ui.Layout(width, height);

            globe.Render(renderer, width, height, present: false);
            ui.Render(renderer);
            SDL.SDL_RenderPresent(renderer);
        }

        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
        return 0;
    }

    private static UIElement BuildUI(StateStore state, Animator animator)
    {
        var zone = state.Get<Zone?>("selectedZone", null);
        float anim = state.Get("panelAnim", 0f);
        bool dismissing = state.Get("panelDismissing", false);

        if (zone == null || anim <= 0f)
            return UIElement.Panel();

        int panelWidth = 260;
        int xOffset = (int)((1f - anim) * panelWidth);

        return UIElement.Panel(
            style: new UIStyle
            {
                Width = panelWidth,
                Padding = 12,
                Gap = 8,
                Anchor = Anchor.TopRight,
                OffsetX = xOffset,
                OffsetY = 20,
                BackgroundColor = Color.FromRgba(20, 25, 40, 220),
                BorderColor = Color.FromRgba(80, 120, 180, 180),
                BorderWidth = 1,
            },
            key: "zone-panel",
            UIElement.Row(
                style: new UIStyle { Direction = LayoutDirection.Horizontal, Gap = 8 },
                key: "zone-header",
                UIElement.Label(zone.Name, style: new UIStyle
                {
                    FontSize = 16,
                    TextColor = Color.FromRgb(240, 240, 255),
                }),
                UIElement.Button("X", () =>
                {
                    if (!dismissing)
                    {
                        state.Set("panelDismissing", true);
                        animator.Animate(anim, 0f, 200f, EaseFunction.EaseIn,
                            v => state.Set("panelAnim", v),
                            () =>
                            {
                                state.Set<Zone?>("selectedZone", null);
                                state.Set("panelDismissing", false);
                            });
                    }
                }, style: new UIStyle
                {
                    Padding = 4,
                    BackgroundColor = Color.FromRgba(120, 40, 40, 200),
                    TextColor = Color.FromRgb(255, 200, 200),
                    BorderColor = Color.FromRgba(180, 80, 80, 180),
                    BorderWidth = 1,
                })
            ),
            UIElement.Label($"Type: {zone.Type}", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(180, 190, 210),
            }),
            UIElement.Label($"Parent: {zone.ParentName ?? "none"}", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(180, 190, 210),
            }),
            UIElement.Label($"Lat: {zone.Centroid.Lat:F1}  Lon: {zone.Centroid.Lon:F1}", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(160, 170, 190),
            })
        );
    }
}
