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
        var adjacency = AdjacencyMap.LoadFromBordersDirectory();
        var globe = new GlobeRenderer(earth);
        var ui = new UIContext();
        var animator = new Animator();
        var state = new StateStore();
        var sim = new SimLoop();
        sim.AddSystem(new XenoGrowthSystem());
        var resources = new ResourceService();
        sim.AddSystem(new BiomassHarvestSystem(resources));
        var game = new GameState();

        ServiceLocator.Register(resources);
        uint lastTick = SDL.SDL_GetTicks();

        globe.ZoneSelected += zone =>
        {
            if (game.Phase == GamePhase.SelectingRegion)
            {
                if (state.Get<Zone?>("confirmZone", null) != null)
                {
                    state.Set<Zone?>("confirmZone", null);
                    return;
                }
                if (zone == null) return;
                if (zone.Type == ZoneType.Country)
                {
                    state.Set("confirmZone", zone);
                    state.Set("modalAnim", 1f);
                }
            }
            else
            {
                if (state.Get<Zone?>("spreadTarget", null) != null)
                {
                    state.Set<Zone?>("spreadTarget", null);
                    state.Set("spreadSources", new List<Entity>());
                    return;
                }
                if (state.Get<Zone?>("upgradeZone", null) != null)
                {
                    state.Set<Zone?>("upgradeZone", null);
                    return;
                }

                if (zone == null) return;

                var infectedEntity = sim.World.EntitiesInZone(zone.Name)
                    .FirstOrDefault(e => sim.World.Has<InfectionComponent>(e), Entity.None);
                bool isInfected = infectedEntity != Entity.None;

                if (isInfected)
                {
                    state.Set("upgradeZone", zone);
                    state.Set("upgradeEntity", infectedEntity);
                    return;
                }

                if (zone.Type == ZoneType.Country)
                {
                    var neighbors = adjacency.GetNeighbors(zone.Name);
                    var sourceEntities = new List<Entity>();
                    foreach (var neighborName in neighbors)
                    {
                        foreach (var e in sim.World.EntitiesInZone(neighborName))
                        {
                            if (sim.World.Has<InfectionComponent>(e))
                            {
                                sourceEntities.Add(e);
                                break;
                            }
                        }
                    }

                    if (sourceEntities.Count > 0)
                    {
                        state.Set("spreadTarget", zone);
                        state.Set("spreadSources", sourceEntities);
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
            ui.Update(() => BuildUI(state, animator, game, sim, adjacency, earth));
            ui.Layout(w, h);
            SyncInfectedZones(globe, sim);
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

            double deltaDays = deltaMs / 1000.0;
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

            ui.Update(() => BuildUI(state, animator, game, sim, adjacency, earth));
            ui.Layout(width, height);

            SyncInfectedZones(globe, sim);
            globe.Render(renderer, width, height, present: false);
            ui.Render(renderer);
            SDL.SDL_RenderPresent(renderer);
        }

        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
        return 0;
    }

    private static UIElement BuildUI(StateStore state, Animator animator, GameState game, SimLoop sim, AdjacencyMap adjacency, Earth earth)
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
            children.Add(BuildTopBar(sim, earth));
            children.Add(BuildTimeControls(sim));

            var spreadTarget = state.Get<Zone?>("spreadTarget", null);
            if (spreadTarget != null)
            {
                var sources = state.Get<List<Entity>>("spreadSources", new List<Entity>());
                children.Add(BuildSpreadModal(state, sim, spreadTarget, sources));
            }
            var upgradeZone = state.Get<Zone?>("upgradeZone", null);
            if (upgradeZone != null)
            {
                var upgradeEntity = state.Get("upgradeEntity", Entity.None);
                children.Add(BuildUpgradeModal(state, sim, upgradeZone, upgradeEntity));
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

    private static UIElement BuildTopBar(SimLoop sim, Earth earth)
    {
        var resources = ServiceLocator.Get<ResourceService>();
        int infectedCount = sim.World.Query<InfectionComponent>().Count();
        int totalCountries = earth.GetByType(ZoneType.Country).Count();
        double elapsedDays = sim.State.ElapsedDays;

        int years = (int)(elapsedDays / 365.25);
        int days = (int)(elapsedDays % 365.25);
        string timeStr = years > 0 ? $"Year {years}, Day {days}" : $"Day {days}";

        return UIElement.Row(
            style: new UIStyle
            {
                Direction = LayoutDirection.Horizontal,
                Gap = 24,
                Padding = 10,
                Anchor = Anchor.TopLeft,
                OffsetX = 0,
                OffsetY = 0,
                Width = 4000,
                Height = 36,
                BackgroundColor = Color.FromRgba(10, 12, 25, 230),
                BorderColor = Color.FromRgba(60, 80, 60, 180),
                BorderWidth = 1,
            },
            key: "top-bar",
            UIElement.Label($"Biomass: {resources.Biomass:F0}", style: new UIStyle
            {
                FontSize = 14,
                TextColor = Color.FromRgb(140, 220, 140),
            }),
            UIElement.Label($"Infected: {infectedCount}/{totalCountries}", style: new UIStyle
            {
                FontSize = 14,
                TextColor = Color.FromRgb(180, 140, 220),
            }),
            UIElement.Label(timeStr, style: new UIStyle
            {
                FontSize = 14,
                TextColor = Color.FromRgb(180, 200, 220),
            })
        );
    }

    private static UIElement BuildTimeControls(SimLoop sim)
    {
        var speeds = new (string label, double scale)[]
        {
            ("x0", 0), ("x1", 1), ("x3", 3), ("x10", 10), ("x30", 30)
        };

        bool isPaused = sim.Paused;
        double currentScale = sim.TimeScale;

        var buttons = new UIElement[speeds.Length];
        for (int i = 0; i < speeds.Length; i++)
        {
            var (label, scale) = speeds[i];
            bool active = (scale == 0 && isPaused) || (scale > 0 && !isPaused && Math.Abs(currentScale - scale) < 0.01);
            double capturedScale = scale;

            buttons[i] = UIElement.Button(label, () =>
            {
                if (capturedScale == 0)
                {
                    sim.Paused = true;
                }
                else
                {
                    sim.Paused = false;
                    sim.TimeScale = capturedScale;
                }
            }, style: new UIStyle
            {
                Padding = 4,
                FontSize = 12,
                BackgroundColor = active
                    ? Color.FromRgba(60, 140, 80, 240)
                    : Color.FromRgba(40, 50, 70, 220),
                TextColor = active
                    ? Color.FromRgb(220, 255, 220)
                    : Color.FromRgb(160, 170, 190),
                BorderColor = active
                    ? Color.FromRgba(100, 200, 120, 220)
                    : Color.FromRgba(70, 90, 110, 180),
                BorderWidth = 1,
            });
        }

        return UIElement.Row(
            style: new UIStyle
            {
                Direction = LayoutDirection.Horizontal,
                Gap = 4,
                Padding = 6,
                Anchor = Anchor.TopRight,
                OffsetX = -6,
                OffsetY = 3,
            },
            key: "time-controls",
            buttons
        );
    }

    private static UIElement BuildConfirmModal(StateStore state, Animator animator, GameState game, SimLoop sim, Zone zone)
    {
        return UIElement.Panel(
            style: new UIStyle
            {
                Anchor = Anchor.Fill,
                BackgroundColor = Color.FromRgba(0, 0, 0, 160),
            },
            key: "confirm-backdrop",
            UIElement.Panel(
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
        ));
    }

    private static UIElement BuildSpreadModal(StateStore state, SimLoop sim, Zone target, List<Entity> sourceEntities)
    {
        var modalChildren = new List<UIElement>();

        modalChildren.Add(UIElement.Label($"Spread to {target.Name}?", style: new UIStyle
        {
            FontSize = 16,
            TextColor = Color.FromRgb(220, 255, 220),
        }));

        if (sourceEntities.Count > 1)
        {
            modalChildren.Add(UIElement.Label("Choose source zone:", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(180, 200, 190),
            }));
        }

        foreach (var sourceEntity in sourceEntities)
        {
            var sourceInfection = sim.World.Get<InfectionComponent>(sourceEntity);
            var sourceLink = sim.World.Get<ZoneLink>(sourceEntity);
            string sourceName = sourceLink?.ZoneName ?? "???";
            double sourceBiomass = sourceInfection?.Biomass ?? 0;
            double afterTax = sourceBiomass * 0.6;
            double each = afterTax / 2.0;

            var capturedEntity = sourceEntity;
            modalChildren.Add(UIElement.Panel(
                style: new UIStyle
                {
                    Padding = 8,
                    Gap = 4,
                    BackgroundColor = Color.FromRgba(25, 35, 50, 200),
                    BorderColor = Color.FromRgba(60, 140, 60, 150),
                    BorderWidth = 1,
                },
                key: $"source-{sourceName}",
                UIElement.Label($"From: {sourceName} ({sourceBiomass:F1} biomass)", style: new UIStyle
                {
                    FontSize = 12,
                    TextColor = Color.FromRgb(160, 180, 170),
                }),
                UIElement.Label($"40% tax. Each zone gets {each:F1}.", style: new UIStyle
                {
                    FontSize = 11,
                    TextColor = Color.FromRgb(140, 160, 150),
                }),
                UIElement.Button("Spread", () =>
                {
                    var infection = sim.World.Get<InfectionComponent>(capturedEntity);
                    if (infection != null)
                    {
                        double remaining = infection.Biomass * 0.6;
                        double half = remaining / 2.0;
                        infection.Biomass = half;

                        var newEntity = sim.World.SpawnInZone(target.Name);
                        sim.World.Set(newEntity, new InfectionComponent { Biomass = half });
                        Console.WriteLine($"Spread to {target.Name} from {sourceName}: {half:F1} biomass each (taxed 40%)");
                    }
                    state.Set<Zone?>("spreadTarget", null);
                    state.Set("spreadSources", new List<Entity>());
                }, style: new UIStyle
                {
                    Padding = 6,
                    BackgroundColor = Color.FromRgba(40, 120, 40, 220),
                    TextColor = Color.FromRgb(220, 255, 220),
                    BorderColor = Color.FromRgba(80, 180, 80, 200),
                    BorderWidth = 1,
                })
            ));
        }

        modalChildren.Add(UIElement.Button("Cancel", () =>
        {
            state.Set<Zone?>("spreadTarget", null);
            state.Set("spreadSources", new List<Entity>());
        }, style: new UIStyle
        {
            Padding = 8,
            BackgroundColor = Color.FromRgba(100, 40, 40, 220),
            TextColor = Color.FromRgb(255, 200, 200),
            BorderColor = Color.FromRgba(180, 80, 80, 200),
            BorderWidth = 1,
        }));

        return UIElement.Panel(
            style: new UIStyle
            {
                Anchor = Anchor.Fill,
                BackgroundColor = Color.FromRgba(0, 0, 0, 160),
            },
            key: "spread-backdrop",
            UIElement.Panel(
                style: new UIStyle
                {
                    Width = 360,
                    Padding = 16,
                    Gap = 10,
                    Anchor = Anchor.Center,
                    BackgroundColor = Color.FromRgba(15, 20, 35, 240),
                    BorderColor = Color.FromRgba(80, 180, 80, 200),
                    BorderWidth = 2,
                },
                key: "spread-modal",
                modalChildren.ToArray()
            )
        );
    }

    private static UIElement BuildUpgradeModal(StateStore state, SimLoop sim, Zone zone, Entity entity)
    {
        var infection = sim.World.Get<InfectionComponent>(entity);
        if (infection == null)
        {
            state.Set<Zone?>("upgradeZone", null);
            return UIElement.Panel();
        }

        int cost = InfectionComponent.UpgradeCost(infection.GrowthLevel);
        var resources = ServiceLocator.Get<ResourceService>();
        bool canAfford = resources.Biomass >= cost;
        int nextLevel = infection.GrowthLevel + 1;
        double nextMultiplier = 1.0 + (nextLevel - 1) * 0.5;

        return UIElement.Panel(
            style: new UIStyle
            {
                Anchor = Anchor.Fill,
                BackgroundColor = Color.FromRgba(0, 0, 0, 160),
            },
            key: "upgrade-backdrop",
            UIElement.Panel(
            style: new UIStyle
            {
                Width = 340,
                Padding = 16,
                Gap = 12,
                Anchor = Anchor.Center,
                BackgroundColor = Color.FromRgba(15, 20, 35, 240),
                BorderColor = Color.FromRgba(180, 140, 40, 200),
                BorderWidth = 2,
            },
            key: "upgrade-modal",
            UIElement.Label($"Upgrade {zone.Name}", style: new UIStyle
            {
                FontSize = 16,
                TextColor = Color.FromRgb(255, 220, 140),
            }),
            UIElement.Label($"Biomass: {infection.Biomass:F1}", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(140, 220, 140),
            }),
            UIElement.Label($"Growth Level: {infection.GrowthLevel} -> {nextLevel}", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(160, 180, 170),
            }),
            UIElement.Label($"Growth Multiplier: x{infection.GrowthMultiplier:F1} -> x{nextMultiplier:F1}", style: new UIStyle
            {
                FontSize = 12,
                TextColor = Color.FromRgb(160, 180, 170),
            }),
            UIElement.Label($"Cost: {cost} biomass (you have {resources.Biomass:F0})", style: new UIStyle
            {
                FontSize = 12,
                TextColor = canAfford ? Color.FromRgb(140, 220, 140) : Color.FromRgb(220, 100, 100),
            }),
            UIElement.Row(
                style: new UIStyle { Direction = LayoutDirection.Horizontal, Gap = 12 },
                key: "upgrade-buttons",
                UIElement.Button(canAfford ? "Upgrade" : "Can't Afford", () =>
                {
                    if (canAfford)
                    {
                        var inf = sim.World.Get<InfectionComponent>(entity);
                        if (inf != null)
                        {
                            int c = InfectionComponent.UpgradeCost(inf.GrowthLevel);
                            if (resources.TrySpend(c))
                            {
                                inf.GrowthLevel++;
                                Console.WriteLine($"Upgraded {zone.Name} to growth level {inf.GrowthLevel} (x{inf.GrowthMultiplier:F1})");
                            }
                        }
                    }
                    state.Set<Zone?>("upgradeZone", null);
                }, style: new UIStyle
                {
                    Padding = 8,
                    BackgroundColor = canAfford
                        ? Color.FromRgba(120, 100, 20, 220)
                        : Color.FromRgba(60, 60, 60, 220),
                    TextColor = canAfford
                        ? Color.FromRgb(255, 230, 150)
                        : Color.FromRgb(140, 140, 140),
                    BorderColor = canAfford
                        ? Color.FromRgba(180, 140, 40, 200)
                        : Color.FromRgba(80, 80, 80, 200),
                    BorderWidth = 1,
                }),
                UIElement.Button("Cancel", () =>
                {
                    state.Set<Zone?>("upgradeZone", null);
                }, style: new UIStyle
                {
                    Padding = 8,
                    BackgroundColor = Color.FromRgba(100, 40, 40, 220),
                    TextColor = Color.FromRgb(255, 200, 200),
                    BorderColor = Color.FromRgba(180, 80, 80, 200),
                    BorderWidth = 1,
                })
            )
        ));
    }

    private static void SyncInfectedZones(GlobeRenderer globe, SimLoop sim)
    {
        globe.InfectedZones.Clear();
        globe.ZoneBiomass.Clear();
        foreach (var entity in sim.World.Query<InfectionComponent>())
        {
            var link = sim.World.Get<ZoneLink>(entity);
            if (link != null)
            {
                globe.InfectedZones.Add(link.ZoneName);
                var infection = sim.World.Get<InfectionComponent>(entity);
                if (infection != null)
                    globe.ZoneBiomass[link.ZoneName] = infection.Biomass;
            }
        }
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
