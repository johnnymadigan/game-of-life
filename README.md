![year](https://img.shields.io/badge/2020-lightgrey?style=plastic)
![creators](https://img.shields.io/badge/Johnny%20Madigan-yellow?style=plastic)
![framework](https://img.shields.io/badge/.NET-informational?style=plastic&logo=.NET)

# **Game of Life**
A zero-player, cellular automaton game.

Play in your terminal and configure custom settings to change how the app simulates 'life'.

![Game of Life demonstration](/img/gol.gif)

# **Play**

Build the project in Visual Studio.

In your terminal, navigate to the "netcoreapp3.1" folder: `cd /your/path/game-of-life/Life/Life/bin/Debug/netcoreapp3.1`

![shortcut](/img/shortcut-to-netcoreapp.gif)

Run: `dotnet life.dll`

Hit enter to run using default settings.

If you want to use [custom settings](#custom-settings), you'll need to type some options before you hit enter.

Settings may need to be followed by one or more parameters like so:

```
--argument param --argument param param
```
The app will tell you if you have entered arguments incorrectly and if parameters are out of bounds, invalid, or missing.

# **Custom Settings**

<kbd>--birth</kbd> followed by integers greater or equal to zero like `3` and ranges between two positive integers like `1...9`. You can use both a range along with single integers. This determines the number of neighbours a cell needs to be born.

<kbd>--survival</kbd> followed by integers greater or equal to zero like `1` or a range between two positive integers like `2...3`. You can use both a range along with single integers. This determines the number of neighbours a cell needs to survive.

![setting rules](/img/rules.gif)

---
<kbd>--dimensions</kbd> followed by row & column integers both 4...48 inclusive like `7 11`.  The size of the universe.

![setting dimensions](/img/dimensions.gif)

---
<kbd>--random</kbd> followed by decimals 0...1 inclusive like `0.5`. The probability of any cell to initially be born, making each universe random if no seed was specified (`0.7` is 70%).

![setting random](/img/random.gif)

---
<kbd>--max-update</kbd> followed by integers 1...30 inclusive like `6`. The number of generations per second, speeding up / slowing down the lifetime of the universe.

![setting max-update](/img/max-update.gif)

---
<kbd>--generations</kbd> followed by integers greater than zero like `11`. The number of generations the life will simulate.

![setting generations](/img/generations.gif)

---
<kbd>--neighbour</kbd> followed by *type*, *order*, and *count centre*. The *type* is the type of neighbourhood used, either `moore` or `vonNeumann` (case insensitive). The *order* is the size of the neighbourhood, integers 1...10 (inclusive) **and** less than half of the smallest dimension (out of rows/columns). *Count centre* is a boolean that will include the centre cell as a neighbour, either `true` or `false`. All together this could look like `vonNeumann 6 false`

![setting neighbour](/img/neighbour.gif)

---
<kbd>--seed</kbd> followed by `path/to/your/file.seed` of the initial seed you want to use (relative or absolute paths). Seeds must be '.seed' files with grid values within the default dimensions (16x16) or dimensions specified by you. A seed will disable the *random* chance of a cell being born initially since the seed already determines which cells start alive. The largest value in the seed (e.g. 7,7) requires a universe of size 8x8. This is because rows & columns are 0-based. A seed directory path cannot have `\` instead please change the directory to have only `/` instead. E.g. `path/to/glider.seed` **NOT** `path\to\glider.seed`

![setting seed](/img/seed-glider.gif)

---
<kbd>--output</kbd> followed by `path/to/your/generated/file.seed` to save the last generation of the universe (relative or absolute paths). The output file must be a '.seed' file, please create one so the program can write the last generation to it.

---
<kbd>--periodic</kbd> wraps the universe around as if there is no border around the grid.

![setting periodic](/img/periodic.gif)

---
<kbd>--step</kbd> will wait for you to press <kbd>space</kbd> to show each generation instead of automatically simulating. When the *step mode* is **ON**, it will disable the *update rate* as generations are not updating automatically anymore.

---
<kbd>--ghost</kbd> shows the ghosts of the past 3 generations, fading more and more as they get older.

![setting ghost](/img/ghost.gif)

# **Classes**

![classes](/Life/ClassDiagram1.png)
