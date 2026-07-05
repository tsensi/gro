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

- [x] Make sure the zones form a non-overlapping covering of the globe. This is a complex task. Please analyze it first, then make a plan here, breaking it into several `- [ ]` tasks.
 
- [x] Create a simple immediate-mode UI toolkit inspired by React.
- [ ] Fix zone coverage test failures: OceanData overlaps with continents (Atlantic/South America, Atlantic/Africa, Atlantic/North America, Arctic/Asia) and uncovered gaps
- [ ] Allow selecting a zone. Animate a UI panel in showing the zone's name and stats. There is a close button on the panel to dismiss it, animating it out
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
