Let's make an idle game. You play as an alien organism that lands in a random spot on earth. The world's countries try to stop you, but if you can outpace their attacks, you can eventually overgrow the whole planet and win.

The game is written in pure C#. It should run using SDL with vulkan on this machine.
All the UI will be a custom system inspired by React, meaning we can write immediate-mode style GUI but have it react to changes in the game.

## Zone tests
- [x] Fix any remaining zone coverage test failures

- [x] For every country, draw a number of dots representing biomass. Develop a re-usable component using a dot-based visual counting system that can easily scale over 10 magnitudes using distinct dot shapes. Document the counting shapes in VISUAL_NUMBERS.md
- [x] The visual numbers should always show a 3x3 grid, rounding down by omitting the least-significant element. Update the code and the md file
- [x] Rename "tax" with "attrition" in the docs and the codebase.
- [x] Add a tech tree system. Add tasks here to implement this, configs (JSON), serialization, UI. Break it into individual tasks. The first three techs are "Infection Wave" in five tiers. For now they do nothing. Research happens in a zone, it is NOT global. Multiple zones can research the same thing. Once it is established in a zone, it diffuses throughout neighboring infected countries slowly. Only when it is at 100% is it fully established and available. If starting research, it adds to the already existing diffusion influx. Research never decreases.
  - [x] Core data model: TechDefinition, TechRegistry (JSON loader), ResearchComponent (per-zone progress)
  - [x] Simulation systems: ResearchSystem (advances active research) + TechDiffusionSystem (spreads from established neighbors)
  - [x] JSON config: data/techs.json with Infection Wave tiers 1–5
  - [x] Serialization: save/load ResearchComponent state per zone
  - [x] UI: show tech progress in zone sidebar, allow starting research from tech tree panel
- [ ] When clicking any country, show the side bar with infection status, biomass, and techs. Allow opening the zone's tech tree from there.

## How to run

Prerequisites: .NET 8 SDK, SDL2 (`libSDL2-dev` on Ubuntu/Debian).

```sh
# Build
dotnet build

# Run (opens an SDL window with the globe visualization)
dotnet run --project Gro

# Run tests
dotnet test
```

If `dotnet` reports "No .NET SDKs were found", ensure `~/.dotnet` is on your PATH:
```sh
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$DOTNET_ROOT:$PATH"
```

---

## Appendix A: Zone Data JSON Format

Zone data is stored as one JSON file per zone. The format is inspired by GeoJSON but simplified to match the game's two-level zone hierarchy.

### Coordinate Convention

- Order: **[longitude, latitude]** (GeoJSON standard)
- Precision: **3 decimal places** (0.001° ≈ 111m at equator, ≈79m at 45°N)
- Rings are **closed**: first coordinate must equal last coordinate
- Coordinates are WGS 84 degrees, longitude in [-180, 180], latitude in [-90, 90]
- Antimeridian-crossing polygons use continuous longitude (e.g. 170 → 185 is allowed as 170 → -175)

### Schema

```json
{
  "name": "Canada (Prairies)",
  "type": "Country",
  "parent": "North America",
  "boundary": [
    [-120.000, 60.000],
    [-90.000, 60.000],
    [-80.000, 55.000],
    [-80.000, 51.000],
    [-90.000, 49.000],
    [-120.000, 49.000],
    [-120.000, 54.000],
    [-120.000, 60.000]
  ]
}
```

### Field Reference

| Field      | Type       | Required | Description                                                       |
|------------|------------|----------|-------------------------------------------------------------------|
| `name`     | string     | yes      | Unique zone name, matches in-game display name                    |
| `type`     | string     | yes      | One of: `"Continent"`, `"Country"`, `"OceanBasin"`, `"OceanZone"` |
| `parent`   | string\|null | yes    | Name of parent zone (null for top-level: Continent, OceanBasin)   |
| `boundary` | number[][] | yes      | Array of [lon, lat] pairs forming a closed polygon ring           |

### File Organization

```
zones/
  continents/
    north-america.json
    south-america.json
    europe.json
    africa.json
    asia.json
    oceania.json
    antarctica.json
  countries/
    canada-prairies.json
    canada-british-columbia.json
    france.json
    ...
  ocean-basins/
    atlantic-ocean.json
    pacific-ocean.json
    indian-ocean.json
    southern-ocean.json
    arctic-ocean.json
  ocean-zones/
    eastern-mediterranean.json
    north-sea.json
    ...
```

File names are the zone name lowercased, with spaces and parentheses replaced by hyphens, e.g. `"Canada (Prairies)"` → `canada-prairies.json`.

### Rounding Rule

All coordinates are rounded to exactly 3 decimal places. When two zones share a border, the shared vertices must have **identical** coordinate values after rounding — this is what enables adjacency detection and ensures no gaps or overlaps at shared edges.

### Validation Rules

1. `boundary` must have at least 4 points (3 unique vertices + closing point)
2. First point must equal last point (closed ring)
3. All coordinates rounded to exactly 3 decimal places
4. `type` must be one of the four valid enum values
5. `parent` must be null for top-level zones (Continent, OceanBasin) and non-null for child zones (Country, OceanZone)
6. No duplicate consecutive points (other than the closing point)
