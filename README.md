---
title: <Game of Life>
author: <Johnny> <Madigan> - <n10027190>
date: 11/10/2020
---

## Build Instructions

**To build the program, please:**

1. Download Visual Studio Community 2019 or later, either for Windows 10 or MacOS X.
2. When installing VS community, install .NET desktop development workload.
3. Navigate into the first "Life" folder located with this README and open the "Life.sln" solution in Visual Studio. 
4. To build for Windows, select "Build Solution". You can also use Constrol+Shift+B. Use "Clean Solution" then "Rebuild Solution" to resolve any errors.
5. To build for Mac, select "Build All". You can also use Command+B. Use "Clean All" then "Rebuild All" to resolve any errors.

## Usage 

**To run the program, please...**

1. Open the Terminal (Mac) OR Command Prompt (Windows).
2. Type "cd" followed by a space in the terminal do not press enter yet.
3. Navigate into the "Life" folder located with this README, then into the next "Life" folder, next the "bin" folder, finally the "Debug" folder.
4. You will see a "netcoreapp3.1" folder, copy the directory path to this folder and paste it in the terminal, you can also drag the folder into the terminal directly and it'll automatically paste right in there for you. The command should now look like: cd /Users/path/netcoreapp3.1
5. *IMPORTANT* If the directory has ANY spaces, please put inside quotation marks like so: cd "/Users/path that has spaces/netcoreapp3.1"
6. Now that you're in the right directory, to call the program, always type: dotnet life.dll 
7. You can hit enter to run the program using the default settings, however, if you want to use your own settings you'll need to type some options before you hit enter. 
8. Each option needs an arguments followed by parameters (if needed). These are the avaliable arguments with their needed parameters, see NOTES for detailed info on each of them.

*       --birth         whole numbers above 0 or a range between 2 numbers like 1...9
*       --survival      whole numbers above 0 or a range between 2 numbers like 1...9
*       --dimensions    row column whole numbers 4...48 inclusive
*       --memory        whole numbers 4...512 inclusive
*       --random 0.5    decimal numbers 0...1 inclusive
*       --max-update    whole numbers 1...30 inclusive
*       --neighbour     type order countcentre
*       --seed          path/to/yourfile.seed
*       --output        path/to/yourfile.seed
*       --generations   whole number above 0
*       --periodic      
*       --step
*       --ghost

## Notes 

* An argument always needs "--" before the word. Like this: --argument
* Arguments may need to be followed by one or more parameters, seperate all arguments and parameters with spaces.
* The program will tell you if you parameters are out of bounds, invalid, or missing.
* The program will tell you if you have entered arguments incorrectly.

* The seed file determines the starting cells.
* Seed must be a '.seed' file with grid values within the default dimensions or specified by you.
* The largest value in the seed file for example might be 7 by 7. It may seem like the grid needs to be 7 by 7 however, grid rows and columns count from 0,0 therefore the grid needs to be 8 by 8 to accomodate for a cell at 7,7.
* A seed directory path will not allow "\" in the input, if so, please change the directory to have "/" instead. 
For example, please use: path/to/glider.seed NOT path\to\glider.seed
* A seed will disable the random value since the seed already determines which cells are alive.

* The output file must be a relative or absolute file path to a .seed file, please create one so the program can write the last generation to it.

* The number of generation cycles must be a whole number above 0.

*  The memory needs to be a whole number 4...512 inclusive.

* The maximum update rate must be between whole numbers 1...30 inclusive.

* The rules determine if a cell survives or is born. 
* There is no limit to how many survival or birth parameters are entered, but they must all be whole numbers above 0 or a range between 2 numbers like 1...9

* The neighbourhood determines the area of cells that will be checked for alive cells, if the number of neighbours matches one of the rules, the cell will be born or survive.
* 1st parameter is the type of neighbourhood, either type "moore" or "vonneumann" case insensitive.
* 2nd parameter is the order, which determines how far the neighbourhood spreads, and must be a whole number 1...10 inclusive.
* 3rd parameter is true or false, whether you want to include the host cell / centre cell as one of the neighbours.

* The periodic mode will be switched ON if entered. This means the cells wont stop at the borders, but instead wrap around to the opposite sides of the grid.

* The dimensions argument MUST be followed by 2 whole numbers, first rows then columns. Rows and columns must be between 4...48 inclusive.

* The random factor must be between 0...1 inclusive. This could either be 0, 1, or any decimal number in-between (for example 0.7 is 70%).

* The step mode will be switched ON if entered. This means you will need to press spacebar to cycle through the generations.
* When the step mode is ON, it will disable the update rate as generations are not updating automatically anymore.

*  Ghost mode shows the remains of the past 3 generations fading more and more as they get older