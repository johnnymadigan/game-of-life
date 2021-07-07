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
- [Dependencies](#dependencies)
- [Notes](#notes)

# About

# Build Instructions

**To build the program, please:**

1. Download *Visual Studio Community 2019* or later, for Windows 10 or MacOS: https://visualstudio.microsoft.com/downloads/
2. During *Visual Studio*'s installation, select & install .NET desktop development workload.

![download VS](/img/download-vs.png)

3. Navigate into the first "Life" folder located with **this** README and open the 'Life.sln' solution in *Visual Studio*.

![open](/img/open.gif)

4. To build for Windows, select 'Build Solution'. You can also use Constrol+Shift+B. Use 'Clean Solution' then 'Rebuild Solution' to resolve any errors.
5. To build for Mac, select 'Build All'. You can also use Command+B. Use 'Clean All' then 'Rebuild All' to resolve any errors.

![build](/img/build.gif)

# How to run 

**To run the program, please:**

1. Navigate into the "Life" folder located with this README, then into the next "Life" folder, next the "bin" folder, next the "Debug" folder, then finally into the "netcoreapp3.1" folder.
2. From here, Windows 10 allows you to copy the directory path by clicking on the directory in the bar across the top of the finder window, then right click and copy or Ctrl+C.
3. After copying this directory, open up the Command Prompt by clicking the Windows icon in the bottom left corner of the screen and typing "Command Prompt" or "cmd".
4. After opening up the command prompt, you will want to navigate to the directory of the program, type "cd" followed by a space to change directory, then paste the directory path copied earlier and hit Enter
5. A shortcut is to type "cd" then space, then drag the "netcoreapp3.1" folder into the Command Prompt. This is a quick way to paste the directory.
6. *IMPORTANT* If the directory has ANY spaces, please put this inside quotation marks, so the command will look like (cd "directory\folder name with spaces\file") without the brackets.
7. This whole command should look something like this:

*       cd "directory\folder\folder"

8. Now that you're in the right directory, always type "dotnet life.dll" to call the program. 
9. You can hit enter to run the program using the default settings, however, if you want to use your own settings, see the options & their rules below.

# Usage

**To use the options, please:**

* Type "--" followed by the argument to pass an argument.
* Arguments may need to be followed by one or more parameters, seperate all arguments and parameters with spaces.
* The program will tell you if you parameters are out of bounds, invalid, or missing.
* The program will tell you if you have entered arguments incorrectly.
* Seed must be a ".seed" file with grid values within the default dimensions or specified by you.
* The largest value in the seed file for example might be 7 by 7. It may seem like the grid needs to be 7 by 7 however, grid rows and columns count from 0,0 therefore the grid needs to be 8 by 8 to accomodate for a cell at 7,7.
* A seed directory path will not allow "\" in the input, if so, please change the directory to have "/" instead. For example, please use "path/to/glider.seed" NOT "path\to\glider.seed".
* A seed will disable the random value since the seed already determines which cells are alive.
* The generations must be a positive whole number (above 0).
* The maximum update rate must be between whole numbers 1 and 30 (inclusive).
* The periodic mode will be switched ON if entered. This means the cells wont stop at the borders, but instead wrap around to the opposite sides of the grid.
* The dimensions argument MUST be followed by 2 whole numbers, first rows then columns. Rows and columns must be between 4 and 48 (inclusive).
* The program display file may run into errors, to resolve this, increase/decrease the console's font size before running the program.
* The random chance must be between 0 to 1 inclusive. This could either be 0, 1, or any decimal number in-between (for example 0.7).
* The step mode will be switched ON if entered. This means you will need to press spacebar to cycle through the generations.
* When the step mode is ON, it will disable the update rate as generations are not updating automatically anymore.
* Here is the list of avaliable arguments with their parameter examples:

*       --seed path/to/glider.seed
*       --generations 50
*       --max-update 5
*       --periodic
*       --dimensions 16 16
*       --random 0.5
*       --step

# Dependencies 

# Notes

This program is in memory of Mathematician, John Conway, creator of the original Game of Life.
