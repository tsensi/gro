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
- [ ] Antarctica should be split into 5 zones.
- [ ] The ocean zones are too large, break them into smaller segments.
- [ ] Document the current state of the project with naming and decisions in CLAUDE.md
- [ ] Make sure the zones form a non-overlapping covering of the globe. This is a complex task. Please analyze it first, then make a plan here, breaking it into several `- [ ]` tasks.

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
