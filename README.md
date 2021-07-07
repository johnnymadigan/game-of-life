![unit](https://img.shields.io/badge/CAB201-Programming%20Principles-ff69b4?style=plastic)
![author](https://img.shields.io/badge/Author-Johnny%20Madigan-yellow?style=plastic)
![year](https://img.shields.io/badge/Year-2020-lightgrey?style=plastic)
![lang](https://img.shields.io/badge/Language-C%20Sharp-informational?style=plastic&logo=C%20Sharp)
![framework](https://img.shields.io/badge/Framework-.NET-informational?style=plastic&logo=.NET)
![software](https://img.shields.io/badge/Visual%20Studio-2019/Mac-blueviolet?style=plastic&logo=visual%20studio)

                                   .x88888x.            x*8888x.:*8888: -"888;                                   
                                  :8**888888X.  :>     X   48888X/`8888H/`8888H
                                  f    `888888x./     X8x.  8888X  8888X  8888X;
                                 '       `*88888~     X8888 X8888  88888  88888;
                                  \.    .  `?)X.      '*888!X8888  X8888  X8888;
                                   `~=-^   X88> ~       `?8 `8888  X888X  X888X
                                          X8888  ~      ~"  '888"  X888   X888
                                          488888           !888;  !888;  !888;
                                  .xx.     88888X         888!   888!   888!
                                 '*8888.   '88888>       88"    88"    88"
                                   88888    '8888>        "~     "~     "~
                                   `8888>    `888                       
                                    "8888     8%           Johnny Madigan
                                     `"888x:-"    https://johnnymadigan.github.io/

- [About](#about)
- [Build Instructions](#build-instructions)
- [How to run](#how-to-run)
- [Usage](#usage)
- [Class Diagram](#class-diagram)
- [Dependencies](#dependencies)
- [Notes](#notes)

# About

# Build Instructions

## To build the program:

1. Download *Visual Studio Community 2019* for Windows or MacOS: https://visualstudio.microsoft.com/downloads/
2. During *Visual Studio*'s installation, select & install .NET desktop development workload.

![download VS](/img/download-vs.png)

3. Navigate into the first "Life" folder located with **this** README and open the 'Life.sln' solution in *Visual Studio*.

![open](/img/open.gif)

4. To build for Windows, select 'Build Solution'. You can also use Constrol+Shift+B. Use 'Clean Solution' then 'Rebuild Solution' to resolve any errors.
5. To build for Mac, select 'Build All'. You can also use Command+B. Use 'Clean All' then 'Rebuild All' to resolve any errors.

![build](/img/build.gif)

# How to run 

## To run the program:

1. Launch your OS' terminal.
2. Navigate to the *game-of-life* project folder with the `cd` command. From here navigate into the 'netcoreapp3.1' folder, the relative path will look like `/game-of-life/Life/Life/bin/Debug/netcoreapp3.1`
3. Alternatively you can type `cd` followed by a space, then drag the 'netcoreapp3.1' folder into your terminal and the path should automatically be pasted.

```zsh
foo@bar:~$ cd /your/path/game-of-life/Life/Life/bin/Debug/netcoreapp3.1
```

![shortcut](/img/shortcut-to-netcoreapp.gif)

4. **IMPORTANT** If you save your directory to easily copy&paste for later, check if it contains ANY spaces. If so, please place it inside quotation marks to avoid errors `"your path with s p a c e s/game-of-life/Life/Life/bin/Debug/netcoreapp3.1"`
5. Now that you're in the right directory, to run the program, always type `dotnet life.dll`
6. Hit enter to run the program using the default settings, however, if you want to use your own custom settings, you'll need to type some options before you hit enter. 
7. Each option is called with an argument and some are followed by parameters. See these options in the [Usage](#usage) section.

# Usage

* An argument always needs "--" before the word. Like this: --argument
* Arguments may need to be followed by one or more parameters, **seperate all arguments and parameters with spaces**.
* The program will tell you if you parameters are out of bounds, invalid, or missing.
* The program will tell you if you have entered arguments incorrectly.

## Custom settings:

<kbd>--birth</kbd> followed by integers greater or equal to zero like `3` and ranges between two positive integers like `1...9`. You can use both a range along with single integers. This determines the number of neighbours a cell needs to be born.

---
<kbd>--survival</kbd> followed by integers greater or equal to zero like `1` or a range between two positive integers like `2...3`. You can use both a range along with single integers. This determines the number of neighbours a cell needs to survive.

---
<kbd>--dimensions</kbd> followed by row & column integers both 4...48 inclusive like `7 11`.  The size of the universe.

---
<kbd>--memory</kbd> followed by integers 4...512 inclusive like `6`. Stores generations in memory to detect steady-states.

---
<kbd>--random</kbd> followed by decimals 0...1 inclusive like `0.5`. The probability of any cell to initially be born, making each universe random if no seed was specified (`0.7` is 70%).

---
<kbd>--max-update</kbd> followed by integers 1...30 inclusive like `6`. The number of generations per second, speeding up / slowing down the lifetime of the universe.

---
<kbd>--generations</kbd> followed by integers greater than zero like `11`. The number of generations the life will simulate.

---
<kbd>--neighbour</kbd> followed by *type*, *order*, and *count centre*. The *type* is the type of neighbourhood used, either `moore` or `vonNeumann` (case insensitive). The *order* is the size of the neighbourhood, integers 1...10 (inclusive) **and** less than half of the smallest dimension (out of rows/columns). *Count centre* is a boolean that will include the centre cell as a neighbour, either `true` or `false`. All together this could look like `vonNeumann 6 false`

---
<kbd>--seed</kbd> followed by `path/to/your/file.seed` of the initial seed you want to use (relative or absolute paths). Seeds must be '.seed' files with grid values within the default dimensions (16x16) or dimensions specified by you. A seed will disable the *random* chance of a cell being born initially since the seed already determines which cells start alive. The largest value in the seed (e.g. 7,7) requires a universe of size 8x8. This is because rows & columns are 0-based. A seed directory path cannot have `\` instead please change the directory to have only `/` instead. E.g. `path/to/glider.seed` **NOT** `path\to\glider.seed`

---
<kbd>--output</kbd> followed by `path/to/your/generated/file.seed` to save the last generation of the universe (relative or absolute paths). The output file must be a '.seed' file, please create one so the program can write the last generation to it.

---
<kbd>--periodic</kbd> wraps the universe around as if there is no border around the grid.

---
<kbd>--step</kbd> will wait for you to press <kbd>space</kbd> to show each generation instead of automatically simulating. When the *step mode* is **ON**, it will disable the *update rate* as generations are not updating automatically anymore.

---
<kbd>--ghost</kbd> shows the ghosts of the past 3 generations, fading more and more as they get older.

# Class Diagram

![class diagram](/img/class-diagram.png)

# Dependencies

As this project was developed with *Microsoft*'s *Visual Studio Community 2019*, your OS must be compatible with it so you can build the program. You should also be able to use *VS Professional* or *VS Enterprise*, but since *VS Community* is free and most accessible, it is recommended. Any future releases of *Visual Studio* will require testing.

# Notes

This project is in memory of Mathematician, John Conway, creator of the original *Game of Life*.
