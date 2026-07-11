Let's make an idle game. You play as an alien organism that lands in a random spot on earth. The world's countries try to stop you, but if you can outpace their attacks, you can eventually overgrow the whole planet and win.

The game is written in pure C#. It should run using SDL with vulkan on this machine.
All the UI will be a custom system inspired by React, meaning we can write immediate-mode style GUI but have it react to changes in the game.

## Zone tests
- [x] Fix any remaining zone coverage test failures

- [x] Fix flaky `GamePlaythroughTests.InfectAlgeria_AfterTicks_BiomassAndTimeAdvance` — fails intermittently due to `ServiceLocator` shared static state race when run alongside other tests

- [x] Create a simple immediate-mode UI toolkit inspired by React.

- [x] Allow selecting a zone. Animate a UI panel in showing the zone's name and stats. There is a close button on the panel to dismiss it, animating it out
- [x] Fix zone coverage test failures: OceanData overlaps (Atlantic/South America, Atlantic/Africa, Atlantic/North America, Arctic/Asia) and uncovered Pacific gaps
- [x] Add a simple ECS framework that can attach entities to zones.
- [x] Add a ticker system. Use it to tick all parts of the simulation. Prepare a document SIMULATION.md and update it as the game grows
- [x] Add the "infection" mechanic. In the beginning of the game, the player selects a starting region. They have to confirm the region selection. Add tasks for a modal dialog system here
- [x] Once a starting region is selected, add a Xeno entity there. It begins with a fixed starting value (let's start at 10). Every day of simulated time, the Xeno entity creates a fraction of its current value as new Xeno value (k-factor of exponential growth). This is designed to start slow. The initial value has doubling-times of ten years.
- [x] When the player selects an adjacent region to an infected region (one with Xeno entity), they can infect the new region. This creates a new Xeno entity on the adjacent field. The value from the old field is split between the old and new field paying a fix "tax" of 40% that gets deducted beforehand (to make it meaningful and costly to spread)
- [x] The player farms a "biomass" resource. Add a global resource counter service / system. Use a ServiceLocator pattern
- [x] On every turn, the biomass increases by the number of infected zones. This will later be refined.
- [x] For a fixed amount of biomass, the player can upgrade an infected zone and increase the growth factor there.
- [x] When a modal window for a selected country is open, clicking outside should close the modal window.
- [x] Add an opaque background to the country selection modal windows
- [x] Add a UI top bar that shows the total biomass, number of infected zones (/ total), and the current simulation time
- [x] Add time controls like in Plague Inc to the top right of the UI top bar. Speeds should be x0 (pause), x1, x3, x10, x30
- [x] Every country that is infected should be painted in blue.
- [x] For every country, draw a number of dots representing biomass. Develop a re-usable component using a dot-based visual counting system that can easily scale over 10 magnitudes using distinct dot shapes. Document the counting shapes in VISUAL_NUMBERS.md
- [x] The visual numbers should always show a 3x3 grid, rounding down by omitting the least-significant element. Update the code and the md file
- [x] The country biomass indicators don't update right now. They should. Fix it.
- [x] Allow zooming in and out using the mouse wheel. The closest distance is still 200km above the surface of the planet, the furthest zoom factor is the current one. Make sure to update all raycasting logic.
- [x] Update the country adjacency table. 
  - [x] Find a list of borders using internet search. It must include border length and border types. Reduce it into a minimal format. Then write a CLI tool to extract the country adjacency in our game into the same.
  - [x] Next to `zones` create a folder `borders` that makes border adjacency explicit. Update the game code to use that.
  - [x] Update the zones and borders infos to contain country adjacency information, including border length.
  - [x] In the game, use the adjacency to decide spreadability. When a new country borders more than one infected zone, the player is allowed to decide from which zone to infect. The modal should thus have space for multiple source zones.

This is an idle game. Research classics of the genre like Cookie Clicker
- [x] Research competitor idle games and add a new file IDLE_IDEAS_PLAN.md in this `- [ ]` task format for all the things to add to make this game better
- [x] Research competitor world domination/grand strategy games and add a new file DOMINATION_IDEAS_PLAN.md in this `- [ ]` task format for all the things to add to make this game better
- [x] Time currently doesn't advance. Please fix that.
- [x] Add a integration test that plays through the game, infecting Algeria and ensures that biomass > 0 and time > 0 after a sufficient number of ticks
- [x] The country-scale modal popup should show the biomass in the country.

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
