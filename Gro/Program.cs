using System;
using SDL2;
using Gro.EarthModel;
using Gro.ECS;
using Gro.Infection;
using Gro.Rendering;
using Gro.Services;
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
        var adjacency = new AdjacencyMap(earth.Zones);
        var globe = new GlobeRenderer(earth);
        var ui = new UIContext();
        var animator = new Animator();
        var state = new StateStore();
        var sim = new SimLoop();
        sim.AddSystem(new XenoGrowthSystem());
        var game = new GameState();

        var resources = new ResourceService();
        ServiceLocator.Register(resources);
        uint lastTick = SDL.SDL_GetTicks();

        globe.ZoneSelected += zone =>
        {
            if (zone == null) return;

            if (game.Phase == GamePhase.SelectingRegion)
            {
                if (zone.Type == ZoneType.Country)
                {
                    state.Set("confirmZone", zone);
                    state.Set("modalAnim", 1f);
                }
            }
            else
            {
                bool isInfected = sim.World.EntitiesInZone(zone.Name).Any(e => sim.World.Has<InfectionComponent>(e));
                if (!isInfected && zone.Type == ZoneType.Country)
                {
                    var neighbors = adjacency.GetNeighbors(zone.Name);
                    Entity? sourceEntity = null;
                    foreach (var neighborName in neighbors)
                    {
                        foreach (var e in sim.World.EntitiesInZone(neighborName))
                        {
                            if (sim.World.Has<InfectionComponent>(e))
                            {
                                sourceEntity = e;
                                break;
                            }
                        }
                        if (sourceEntity != null) break;
                    }

                    if (sourceEntity != null)
                    {
                        state.Set("spreadTarget", zone);
                        state.Set("spreadSource", sourceEntity.Value);
                        return;
                    }
                }

                state.Set("selectedZone", zone);
                state.Set("panelDismissing", false);
                animator.Animate(0f, 1f, 250f, EaseFunction.EaseOut,
                    v => state.Set("panelAnim", v));
            }
        };

        if (headless)
        {
            SDL.SDL_GetWindowSize(window, out int w, out int h);
            ui.Update(() => BuildUI(state, animator, game, sim, adjacency));
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

            ui.Update(() => BuildUI(state, animator, game, sim, adjacency));
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

    private static UIElement BuildUI(StateStore state, Animator animator, GameState game, SimLoop sim, AdjacencyMap adjacency)
    {
        var children = new List<UIElement>();

        if (game.Phase == GamePhase.SelectingRegion)
        {
            children.Add(BuildSelectionPrompt());
            var confirmZone = state.Get<Zone?>("confirmZone", null);
            if (confirmZone != null)
            {
                children.Add(BuildConfirmModal(state, animator, game, sim, confirmZone));
            }
        }
        else
        {
            var spreadTarget = state.Get<Zone?>("spreadTarget", null);
            if (spreadTarget != null)
            {
                var sourceEntity = state.Get("spreadSource", Entity.None);
                children.Add(BuildSpreadModal(state, sim, spreadTarget, sourceEntity));
            }
            children.Add(BuildZonePanel(state, animator));
        }

        return UIElement.Panel(
            style: new UIStyle { Width = 0, Height = 0 },
            key: "root",
            children.ToArray()
        );
    }

    private static UIElement BuildSelectionPrompt()
    {
        return UIElement.Panel(
            style: new UIStyle
            {
                Padding = 12,
                Anchor = Anchor.TopLeft,
                OffsetX = 20,
                OffsetY = 20,
                BackgroundColor = Color.FromRgba(20, 25, 40, 220),
                BorderColor = Color.FromRgba(80, 180, 80, 180),
                BorderWidth = 1,
            },
            key: "selection-prompt",
            UIElement.Label("Select a starting region", style: new UIStyle
            {
                FontSize = 16,
                TextColor = Color.FromRgb(180, 255, 180),
            })
        );
    }

    private static UIElement BuildConfirmModal(StateStore state, Animator animator, GameState game, SimLoop sim, Zone zone)
    {
        return UIElement.Panel(
            style: new UIStyle
            {
                Width = 320,
                Padding = 16,
                Gap = 12,
                Anchor = Anchor.Center,
                BackgroundColor = Color.FromRgba(15, 20, 35, 240),
                BorderColor = Color.FromRgba(80, 180, 80, 200),
                BorderWidth = 2,
            },
            key: "confirm-modal",
            UIElement.Label($"Infect {zone.Name}?", style: new UIStyle
            {
                FontSize = 16,
                TextColor = Color.FromRgb(220, 255, 220),
            }),
            UIElement.Label("Your organism will begin growing here.", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(160, 180, 170),
            }),
            UIElement.Row(
                style: new UIStyle { Direction = LayoutDirection.Horizontal, Gap = 12 },
                key: "modal-buttons",
                UIElement.Button("Confirm", () =>
                {
                    game.Phase = GamePhase.Playing;
                    game.StartingZone = zone;
                    var entity = sim.World.SpawnInZone(zone.Name);
                    sim.World.Set(entity, new InfectionComponent { Biomass = 10.0 });
                    state.Set<Zone?>("confirmZone", null);
                    Console.WriteLine($"Infection started in: {zone.Name}");
                }, style: new UIStyle
                {
                    Padding = 8,
                    BackgroundColor = Color.FromRgba(40, 120, 40, 220),
                    TextColor = Color.FromRgb(220, 255, 220),
                    BorderColor = Color.FromRgba(80, 180, 80, 200),
                    BorderWidth = 1,
                }),
                UIElement.Button("Cancel", () =>
                {
                    state.Set<Zone?>("confirmZone", null);
                }, style: new UIStyle
                {
                    Padding = 8,
                    BackgroundColor = Color.FromRgba(100, 40, 40, 220),
                    TextColor = Color.FromRgb(255, 200, 200),
                    BorderColor = Color.FromRgba(180, 80, 80, 200),
                    BorderWidth = 1,
                })
            )
        );
    }

    private static UIElement BuildSpreadModal(StateStore state, SimLoop sim, Zone target, Entity sourceEntity)
    {
        var sourceInfection = sim.World.Get<InfectionComponent>(sourceEntity);
        var sourceLink = sim.World.Get<ZoneLink>(sourceEntity);
        string sourceName = sourceLink?.ZoneName ?? "???";
        double sourceBiomass = sourceInfection?.Biomass ?? 0;
        double afterTax = sourceBiomass * 0.6;
        double each = afterTax / 2.0;

        return UIElement.Panel(
            style: new UIStyle
            {
                Width = 340,
                Padding = 16,
                Gap = 12,
                Anchor = Anchor.Center,
                BackgroundColor = Color.FromRgba(15, 20, 35, 240),
                BorderColor = Color.FromRgba(80, 180, 80, 200),
                BorderWidth = 2,
            },
            key: "spread-modal",
            UIElement.Label($"Spread to {target.Name}?", style: new UIStyle
            {
                FontSize = 16,
                TextColor = Color.FromRgb(220, 255, 220),
            }),
            UIElement.Label($"From: {sourceName} ({sourceBiomass:F1} biomass)", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(160, 180, 170),
            }),
            UIElement.Label($"40% tax. Each zone gets {each:F1} biomass.", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(160, 180, 170),
            }),
            UIElement.Row(
                style: new UIStyle { Direction = LayoutDirection.Horizontal, Gap = 12 },
                key: "spread-buttons",
                UIElement.Button("Spread", () =>
                {
                    var infection = sim.World.Get<InfectionComponent>(sourceEntity);
                    if (infection != null)
                    {
                        double remaining = infection.Biomass * 0.6;
                        double half = remaining / 2.0;
                        infection.Biomass = half;

                        var newEntity = sim.World.SpawnInZone(target.Name);
                        sim.World.Set(newEntity, new InfectionComponent { Biomass = half });
                        Console.WriteLine($"Spread to {target.Name}: {half:F1} biomass each (taxed 40%)");
                    }
                    state.Set<Zone?>("spreadTarget", null);
                }, style: new UIStyle
                {
                    Padding = 8,
                    BackgroundColor = Color.FromRgba(40, 120, 40, 220),
                    TextColor = Color.FromRgb(220, 255, 220),
                    BorderColor = Color.FromRgba(80, 180, 80, 200),
                    BorderWidth = 1,
                }),
                UIElement.Button("Cancel", () =>
                {
                    state.Set<Zone?>("spreadTarget", null);
                }, style: new UIStyle
                {
                    Padding = 8,
                    BackgroundColor = Color.FromRgba(100, 40, 40, 220),
                    TextColor = Color.FromRgb(255, 200, 200),
                    BorderColor = Color.FromRgba(180, 80, 80, 200),
                    BorderWidth = 1,
                })
            )
        );
    }

    private static UIElement BuildZonePanel(StateStore state, Animator animator)
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
