Let's make an idle game. You play as an alien organism that lands in a random spot on earth. The world's countries try to stop you, but if you can outpace their attacks, you can eventually overgrow the whole planet and win.

The game is written in pure C#. It should run using SDL with vulkan on this machine.
All the UI will be a custom system inspired by React, meaning we can write immediate-mode style GUI but have it react to changes in the game.

- [ ] Set up a baseline C# project. Make sure it runs using SDL. Add tasks here to implement it step by step. If anything requires user input, mark it as INPUT NEEDED and mask the task as done using [x]
- [ ] Set up a basic earth model in pure C#, dividing the globe into countries or states using geo-coordinates. Divide the ocean into zones that correspond to their human names, e.g. Eastern Mediterranean. Research and add tasks, starting at the top level hierarchy (continents, oceans) and refining to a cell size around 1000km.
- [ ] Add a simple visualization of the polygons on the globe. In the SDL main window, render the globe showing the outlines. Use something like globe.gl as inspiration. Research, then add tasks to implement this step by step.
