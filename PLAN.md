Let's make an idle game. You play as an alien organism that lands in a random spot on earth. The world's countries try to stop you, but if you can outpace their attacks, you can eventually overgrow the whole planet and win.

The game is written in pure C#. It should run using SDL with vulkan on this machine.
All the UI will be a custom system inspired by React, meaning we can write immediate-mode style GUI but have it react to changes in the game.

- [x] Set up a baseline C# project. Make sure it runs using SDL. Add tasks here to implement it step by step. If anything requires user input, mark it as INPUT NEEDED and mask the task as done using [x]
- [x] Set up a basic earth model in pure C#, dividing the globe into countries or states using geo-coordinates. Divide the ocean into zones that correspond to their human names, e.g. Eastern Mediterranean. Research and add tasks, starting at the top level hierarchy (continents, oceans) and refining to a cell size around 1000km.
- [x] Add a simple visualization of the polygons on the globe. In the SDL main window, render the globe showing the outlines. Use something like globe.gl as inspiration. Research, then add tasks to implement this step by step.
- [x] Document how to run the project here.
- [x] Add a way render each zone in a different style, varying outline color, width, and fill color and width
- [x] Add mouse input, allowing to select any zone on the globe. Log the selected zone to the console.
- [x] Allow rotating the globe with the mouse like in Google Earth. Add tasks here to break this down into simpler sub-tasks.
- [x] Make sure that all of Europe is represented. For each European county, there should be a zone that contains it.
- [x] Make sure that all of Africa is represented.
- [x] Make sure that all of North America is represented. Split Canada into about 5 regions, split the US in 3-4 regions
- [x] Make sure that all of Central America is represented. It's Mexico (split in 3 regions), plus 3 more regions
- [x] Make sure that all of continental Asia is represented. Split large countries like China, Mongolia, Russia, ...
- [x] Make sure that all of Oceania is represented.
- [x] Antarctica should be split into 5 zones.
- [x] The ocean zones are too large, break them into smaller segments.
- [x] Document the current state of the project with naming and decisions in CLAUDE.md

## Zone tests

- [x] Add a --sonnet flag to capi.mjs that uses Sonnet instead of Opus (with the correct pinned model). DO NOT RUN THE SCRIPT!

The tests currently are failing, but running them all overwhelms the the token limits in the code agent. We have to break this down into separate tasks
- [x] Look at Zone.cs Is it too large or complex? Can we extract algorithms like point-inside? If yes, add tasks here to do so
  - Conclusion: No. Zone.cs is 87 lines with a single responsibility. The point-in-polygon, centroid, and antimeridian logic are tightly coupled to Boundary and not reused elsewhere. No extraction needed.
- [x] Change the default behavior of the zone tests to run sequentially for each zone, instead of a large O(n^2) double loop. This means we have to dynamically create test cases for each zone. However, this is fine. They don't have to be harnessed unit tests, this should be a separate CLI tool.
- [x] Define a geo-json inspired JSON format for the zone data. Add it in the Appendix further down. NOTE: The resolution should be on the order of 100m. Round to precision, making it easier to later fix correct tiling and adjacency.
- [x] Extract ContinentData.cs into JSON and load it at app start. Each continent ahoudl be one file.
- [x] Extract CountryData.cs into JSON and load it at app start. Each country zone should be one file.
- [x] Extract OceanData.cs into JSON and load it at app start. Each ocean ZONE should be one file.
- [x] We need better tools to deal with overlaps. Add tasks here to add CLI tools to validate, normalize polygons in the JSON format.
- [x] Create a CLI tool to fix intersections between zones. Add a mode that subtracts one polygon from another.
- [x] Add a "split-difference" mode to the fix-intersection tool. It intersects both polygons, then find the median line in the intersection area and adjusts both polygons to become a compromise solution.
- [x] Now, create tasks for each letter of the alphabet. Run the zone overlap tests only for zones that start with that letter and fix the overlap errors that come up. For each country that comes up, add a task here to run the split-difference mode between two countries. If one intersection partner is an ocean, subtract it from the country zone.
  - [x] Zone overlap fixes: letter A (Africa, Asia, Antarctica, Albania, Algeria, Angola, Afghanistan, Argentina, Australia, Austria, Arabia zones, Arctic/Atlantic/Andaman ocean zones)
  - [x] Zone overlap fixes: letter B (Belgium, Belarus, Bangladesh, Benin, Bhutan, Bosnia, Botswana, Brazil, Bulgaria, Burkina Faso, Burundi, Baltic/Barents/Bay/Beaufort/Bering/Black ocean zones)
  - [x] Zone overlap fixes: letter C (Canada regions, Cameroon, Cambodia, Chad, China regions, Colombia, Costa Rica-Panama, Croatia, Cuba, Czech Republic, Caribbean/Central/Coral ocean zones)
  - [x] Zone overlap fixes: letter D (Denmark, Djibouti, DR Congo)
  - [x] Zone overlap fixes: letter E (Egypt, Equatorial Guinea, Eritrea, Estonia, Eswatini, Ethiopia, Europe, East China Sea, Eastern Mediterranean, East Siberian Sea, Equatorial Pacific)
  - [x] Zone overlap fixes: letter F (Fiji, Finland, France, French Polynesia)
  - [x] Zone overlap fixes: letter G (Gabon, Gambia, Georgia-Armenia-Azerbaijan, Germany, Ghana, Greece, Greenland, Guatemala-Belize, Guinea, Guinea-Bissau, Gulf/Greenland ocean zones)
  - [x] Zone overlap fixes: letter H (Haiti-Dominican Republic, Honduras-El Salvador-Nicaragua, Hungary)
  - [x] Zone overlap fixes: letter I (Iceland, India, Indonesia, Iran, Iraq, Ireland, Italy, Ivory Coast, Indian Ocean)
  - [x] Zone overlap fixes: letter J (Jamaica, Japan, Jordan)
  - [x] Zone overlap fixes: letter K (Kazakhstan, Kenya, Kyrgyzstan, Kara Sea)
  - [x] Zone overlap fixes: letter L (Laos, Latvia, Lebanon-Israel, Lesotho, Liberia, Libya, Lithuania, Laptev Sea)
  - [x] Zone overlap fixes: letter M (Madagascar, Malawi, Malaysia, Mali, Mauritania, Mexico regions, Micronesia, Moldova, Mongolia, Montenegro, Morocco, Mozambique, Myanmar, Mediterranean/Mozambique Channel ocean zones)
  - [x] Zone overlap fixes: letter N (Namibia, Nepal, Netherlands, New Zealand, Niger, Nigeria, North Korea, North Macedonia, Norway, North America, North Sea, Northeast/Northwest/Norwegian ocean zones)
  - [x] Zone overlap fixes: letter O (Oceania, Oman)
  - [x] Zone overlap fixes: letter P (Pakistan, Papua New Guinea, Philippines, Poland, Portugal, Pacific Ocean, Persian Gulf, Philippine Sea)
  - [x] Zone overlap fixes: letter R (Republic of Congo, Romania, Russia regions, Rwanda, Red Sea)
  - [x] Zone overlap fixes: letter S (Samoa-Tonga, Saudi Arabia, Senegal, Serbia, Sierra Leone, Slovakia, Slovenia, Solomon Islands, Somalia, South Africa, South Korea, South Sudan, Spain, Sri Lanka, Sudan, Sweden, Switzerland, Syria, South America, Southern Ocean, Sargasso/South China/Southeast/Southwest ocean zones)
  - [x] Zone overlap fixes: letter T (Taiwan, Tajikistan, Tanzania, Thailand, Togo, Tunisia, Turkey, Turkmenistan, Tasman Sea)
  - [x] Zone overlap fixes: letter U (UAE-Qatar-Kuwait-Bahrain, Uganda, Ukraine, United Kingdom, United States regions, Uzbekistan)
  - [x] Zone overlap fixes: letter V (Vanuatu-New Caledonia, Vietnam)
  - [ ] Zone overlap fixes: letter W (Western Mediterranean)
  - [ ] Zone overlap fixes: letter Y (Yemen)
  - [ ] Zone overlap fixes: letter Z (Zambia, Zimbabwe)
- [ ] Now, after fixing, create tasks again for each letter of the alphabet and run the tests only for zones with that letter. Fix any remaining issues.
- [ ] Use the improved tooling to make sure the zones form a non-overlapping covering of the globe. This is a complex task. Please analyze it first, then make a plan here, breaking it into several `- [ ]` tasks.
- [ ] Fix any remaining zone coverage test failures

- [x] Create a simple immediate-mode UI toolkit inspired by React.

- [x] Allow selecting a zone. Animate a UI panel in showing the zone's name and stats. There is a close button on the panel to dismiss it, animating it out
- [ ] Fix zone coverage test failures: OceanData overlaps (Atlantic/South America, Atlantic/Africa, Atlantic/North America, Arctic/Asia) and uncovered Pacific gaps
- [ ] Add a simple ECS framework that can attach entities to zones.
- [ ] Add a ticker system. Use it to tick all parts of the simulation. Prepare a document SIMULATION.md and update it as the game grows
- [ ] Add the "infection" mechanic. In the beginning of the game, the player selects a starting region. They have to confirm the region selection. Add tasks for a modal dialog system here
- [ ] Once a starting region is selected, add a Xeno entity there. It begins with a fixed starting value (let's start at 10). Every day of simulated time, the Xeno entity creates a fraction of its current value as new Xeno value (k-factor of exponential growth). This is designed to start slow. The initial value has doubling-times of ten years.
- [ ] When the player selects an adjacent region to an infected region (one with Xeno entity), they can infect the new region. This creates a new Xeno entity on the adjacent field. The value from the old field is split between the old and new field paying a fix "tax" of 40% that gets deducted beforehand (to make it meaningful and costly to spread)
- [ ] The player farms a "biomass" resource. Add a global resource counter service / system. Use a ServiceLocator pattern
- [ ] On every turn, the biomass increases by the number of infected zones. This will later be refined.
- [ ] For a fixed amount of biomass, the player can upgrade an infected zone and increase the growth factor there.

This is an idle game. Research classics of the genre like Cookie Clicker
- [ ] Research competitor idle games and add a new file IDLE_IDEAS_PLAN.md in this `- [ ]` task format for all the things to add to make this game better
- [ ] Research competitor world domination/grand strategy games and add a new file DOMINATION_IDEAS_PLAN.md in this `- [ ]` task format for all the things to add to make this game better


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
