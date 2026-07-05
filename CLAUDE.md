# Gro

Idle game where you play as an alien organism that lands on Earth and tries to overgrow the planet. Countries fight back; you win by outpacing their attacks.

## Tech Stack

- Pure C# / .NET 8
- SDL2 via `ppy.SDL2-CS` NuGet package (rendering, input)
- xunit for tests
- No game engine — custom rendering and (planned) custom React-inspired immediate-mode UI

## Build & Run

```sh
dotnet build
dotnet run --project Gro          # opens SDL window
dotnet run --project Gro -- --headless  # renders one frame, exits
dotnet test                       # xunit tests
```

Requires: .NET 8 SDK, SDL2 (`libSDL2-dev`).

If `dotnet` isn't found: `export DOTNET_ROOT="$HOME/.dotnet" && export PATH="$DOTNET_ROOT:$PATH"`

## Project Layout

```
Gro/                        # Main executable
  Program.cs                # Entry point, SDL loop
  EarthModel/
    Earth.cs                # Top-level model: holds all zones, lookup methods
    Zone.cs                 # A polygon on the globe (boundary + point-in-polygon test)
    ZoneType.cs             # Continent | Country | OceanBasin | OceanZone
    GeoCoord.cs             # Lat/lon record struct with haversine distance
    ContinentData.cs        # 7 continent boundary polygons
    CountryData.cs          # ~120 country/region polygons (hand-authored coordinates)
    OceanData.cs            # 5 ocean basins + ~56 ocean sub-zones
  Rendering/
    GlobeRenderer.cs        # Orthographic globe projection, mouse interaction, scanline fill
    ZoneStyle.cs            # Color + outline/fill properties per zone
    ZoneStyleProvider.cs    # Deterministic style assignment by zone type + name hash
Gro.Tests/                  # xunit test project
  EarthModelTests.cs        # Zone hierarchy, point-in-polygon, lookup tests
  ZoneCoverageTests.cs      # Globe coverage and overlap checks (grid + capital city tests)
Gro.ZoneCheck/              # CLI tool for sequential zone validation
  Program.cs                # Overlap, coverage, hierarchy, and capital checks per zone
Gro.sln                     # Solution file
PLAN.md                     # Task list driving development
capi.mjs                    # External automation script (gitignored, not part of the game)
```

## Architecture Decisions

**Zone hierarchy**: Two-level — top-level zones (Continent, OceanBasin) tile the globe without overlap; child zones (Country, OceanZone) nest inside via `ParentName`. `Earth.ZoneAt()` searches most-specific first.

**Coordinate data**: All zone boundaries are hand-authored GeoCoord arrays in static `*Data.cs` files. No external geodata files or shapefiles. Large countries are split into named regions (e.g. "Russia (European)", "China (North)").

**Rendering**: Orthographic projection drawn entirely with SDL2 line/scanline primitives. No GPU shaders, no texture maps. Globe rotates via mouse drag or arrow keys.

**Naming conventions**: Zone names use natural English names. Multi-country groupings use hyphens: "UAE-Qatar-Kuwait-Bahrain", "Honduras-El Salvador-Nicaragua". Sub-regions use parenthetical qualifiers: "Canada (Prairies)", "United States (Northeast)".

## Testing

Tests verify:
- Zone hierarchy integrity (all countries have valid continent parent, all ocean zones have valid basin parent)
- Point-in-polygon correctness (capital cities resolve to correct zones)
- Globe coverage (grid sampling confirms no gaps or overlaps at top level)
- Known distances (haversine sanity check)

Run `dotnet test` before committing. The coverage tests use grid sampling at 5-degree and 10-degree intervals.

The `Gro.ZoneCheck` CLI runs zone validation sequentially per zone (avoiding the O(n^2) double loops in xunit):
```sh
dotnet run --project Gro.ZoneCheck              # all zones
dotnet run --project Gro.ZoneCheck -- A         # only zones starting with 'A'
dotnet run --project Gro.ZoneCheck -- --verbose # show passing checks too
```
