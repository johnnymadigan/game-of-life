using System;
using Display;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Life
{
    /// <summary>
    /// Brings the universe to life
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    class Universe
    {
       /*-----------------------------------------------------------------
                     GLOBAL AND SHARED PRIVATE VARIABLES
                           FOR MUTATING THE CELLS
        -----------------------------------------------------------------*/
        // STORING CERTAIN CELLS IN ARRAYS AND LISTS 
        private static int[,] storeAliveCells = { { 0, 0 } };
        private static int[,] storeAllCells = { { 0, 0 } };
        public static List<int[,]> steadyMemories = new List<int[,]>();
        public static List<List<List<int>>> ghostMemories = new List<List<List<int>>>();

        // DIMENSIONS VARIABLES
        public static int rows = 16;
        public static int cols = 16;
        public static int totalCells = 256; // 16 x 16

        // RANDOM FACTOR VARIABLES
        public static double randomFactor = 0.5;

        // GENERATIIONS VARIABLES
        public static int generationsCount = 50;

        // UPDATE RATE VARIABLES
        public static int minuteToMilliseconds = 1000;
        public static int updatesPerSecond = 5;

        // PERIODIC & STEP BOOLS
        public static bool stepModeOn = false;
        public static bool periodicOn = false;

        // OUTPUT VARIABLES
        public static string outputPathString = "";
        public static bool outputSuccess = false; 
        public static bool seedSuccess = false;
        public static int[,] storeSeedCells = { { 0, 0 } };

        // RULES LISTS
        public static List<int> birth = new List<int>() { 3 };
        public static List<int> survival = new List<int>() { 2, 3 };

        // NEIGHBOURHOOD VARIABLES
        public static int order = 1;
        public static bool von = false;
        public static bool countCentre = false;

        // GHOST MODE VARIABLES
        public static bool ghostOn = false;
        public static int ghostLimit = 3;

        // STEADY-STATE VARIABLES
        public static int steadyLimit = 16;
        public static bool steadyStateDetected = false;

        /// <summary>
        /// 1st wait for user to press the space-bar key to begin
        /// 2nd run the simulation based on the user's input or defaults
        /// 3rd if step mode is on, user must press the space-bar key to show each generation
        /// 4th if step mode is off, each generation will be updated at the refresh rate
        /// 5th foreach generation, grab all the cells and check their neighbours
        /// 6th if their neighbours match the rules, the cell can survive or be born
        /// 7th if ghost mode is on, store the previous generation, once 3 generations are stored
        /// remove the oldest and replace it with the new one to keep it at 3 stored generations
        /// 8th clear the grid and update the grid with the new generation, if ghost mode is
        /// on, update the grid with the old faded generations too
        /// 9th once the simulation is complete, the program will wait for the user to
        /// press the space-bar key to clear the screen and end the program
        /// </summary>
        public static void BringToLife()
        {
            /*------------------------------------------------------------
                                    SETUP THE UNIVERSE
             ------------------------------------------------------------*/
            storeAllCells = UniverseSetup.GenerateAllCells(totalCells, rows, cols);

            //if seed is successful set the initial cells to be the seed
            //else the alive cells are randomized
            if (seedSuccess)
            {
                storeAliveCells = storeSeedCells;
            }
            else
            {
                storeAliveCells = UniverseSetup.GenerateRandomCells(totalCells, randomFactor,
                    storeAllCells);
            }

            /*------------------------------------------------------------
                  CONSTRUCT THE GRID & GENERATE THE INITIAL LIVE CELLS
                                    SEED OR RANDOMISED 
             ------------------------------------------------------------*/
            // Construct new grid & wait for the user to press spacebar to begin
            Grid grid = new Grid(rows, cols);

            Console.WriteLine(" Press spacebar to start simulation...");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar){}

            // Initialize the grid window (this will resize the window and buffer)
            grid.InitializeWindow();

            // Set the footnote (appears in the bottom left of the screen)
            grid.SetFootnote("Iteration: 0");

            // Update the grid with the intial live cells...
            for (int cell = 0; cell < storeAliveCells.GetLength(0); cell++)
            {
                grid.UpdateCell(storeAliveCells[cell, 0], storeAliveCells[cell, 1], CellState.Full);
            }

            // Render updates to the console window...
            grid.Render();

            /*------------------------------------------------------------
                    BEGIN MUTATING CELLS FOR EACH GENERATION BELOW
             ------------------------------------------------------------*/
            // New stopwatch
            Stopwatch watch = new Stopwatch();

            // Grab the initial alive cells array and the all cells array
            int[,] checkAllCells = storeAllCells;
            int[,] checkAliveCells = storeAliveCells;

            // Loop for the generation amount
            for (int generationCycle = 0; generationCycle < generationsCount; generationCycle++)
            {
                List<List<int>> nextGenerationOfCells = new List<List<int>>();

                /*------------------------------------------------------------
                    IF STEP MODE IS ON, WAIT FOR THE USER TO PRESS SPACEBAR
                        OTHERWISE MUTATE THE CELLS AT THE REFRESH RATE
                 ------------------------------------------------------------*/
                // If step mode is on, press spacebar key to go through the generations
                if (stepModeOn)
                {
                    while (Console.ReadKey().Key != ConsoleKey.Spacebar){}
                }
                // If step mode is off, update generations at the speed of the refresh rate
                else
                {
                    watch.Restart();

                    while (watch.ElapsedMilliseconds < (minuteToMilliseconds / updatesPerSecond)) ;
                }

                /*------------------------------------------------------------
                                   UPDATE FOOTNOTE ITERATION
                 ------------------------------------------------------------*/
                // Update iterations to count each generation
                string footNote = "Iteration: " + (generationCycle + 1);
                grid.SetFootnote(footNote);

                /*------------------------------------------------------------
                                    LOOP TO GRAB EACH CELL
                          THEN CHECK THE NEIGHBOURHOOD FOR THAT CELL
                 ------------------------------------------------------------*/
                // For every cell in the grid, dead or alive...
                for (int allCell = 0; allCell < totalCells; allCell++)
                {
                    bool cellFound = false;

                    int length = checkAliveCells.Length / 2;

                    // Check if the cell is in the alive cells array
                    for (int aliveCell = 0; aliveCell < length; aliveCell++)
                    {
                        if (checkAllCells[allCell, 0] == checkAliveCells[aliveCell, 0] &&
                            checkAllCells[allCell, 1] == checkAliveCells[aliveCell, 1])
                        {
                            cellFound = true;
                            break;
                        }
                    }

                    // Variables to hold the x & y value of the cell
                    int xValue = checkAllCells[allCell, 0];
                    int yValue = checkAllCells[allCell, 1];
                    int aliveCount = 0;

                    /*------------------------------------------------------------
                                         CHECK NEIGHBOURHOOD
                     ------------------------------------------------------------*/ 
                    // If cell is alive, check neighbours
                    // Alive cells need to match the rules to survive
                    if (cellFound)
                    {
                        aliveCount = SubNeighbourhood.checkNeighbours(checkAllCells, checkAliveCells,
                            xValue, yValue, rows, cols, order, von, countCentre, periodicOn);
                        if (survival.Contains(aliveCount))
                        {
                            nextGenerationOfCells.Add(new List<int>() { xValue, yValue });
                        }
                    }
                    // If cell is dead, check neigbours
                    // Dead cells need to match the rules to be born
                    else
                    {
                        aliveCount = SubNeighbourhood.checkNeighbours(checkAllCells, checkAliveCells,
                            xValue, yValue, rows, cols, order, von, countCentre, periodicOn);
                        if (birth.Contains(aliveCount))
                        {
                            nextGenerationOfCells.Add(new List<int>() { xValue, yValue });
                        }
                    }
                }

                // Add all the next generation's live cells to an array
                int[,] nextGenerationOfCellsArray = new int[nextGenerationOfCells.Count, 2];
                for (int cell = 0; cell < nextGenerationOfCells.Count; cell++)
                {
                    nextGenerationOfCellsArray[cell, 0] = nextGenerationOfCells[cell][0];
                    nextGenerationOfCellsArray[cell, 1] = nextGenerationOfCells[cell][1];
                }

                /*------------------------------------------------------------
                                         STEADY-STATE
                 ------------------------------------------------------------*/
                // Steady-state not detecting for some reason :(
                // Command line arguments for steady state memory work though
                

                /*------------------------------------------------------------
                                  UPDATE GHOST MODE MEMORY
                 ------------------------------------------------------------*/
                List<List<int>> currentMemoryForGhost = new List<List<int>>();

                for (int cell = 0; cell < checkAliveCells.Length / 2; cell++)
                {
                    currentMemoryForGhost.Add(new List<int>() { checkAliveCells[cell, 0], checkAliveCells[cell, 1] });
                }

                int ghostMemoryUsed = 0;

                foreach (List<List<int>> item in ghostMemories)
                {
                    ghostMemoryUsed++;
                }

                if (ghostMemoryUsed < ghostLimit)
                {
                    ghostMemories.Add(currentMemoryForGhost);
                }
                else
                {
                    ghostMemories.RemoveAt(0);
                    ghostMemories.Add(currentMemoryForGhost);
                }

                /*------------------------------------------------------------
                          STORE THE NEW ALIVE CELLS TO BE USED LATER
                              TO REPEAT THE MUTATION PROCESS &
                       CLEAR THE GRID FOR THE NEXT GENERATION OF CELLS
                 ------------------------------------------------------------*/
                // Store the new alive cells to be used later to repeat the mutation process
                checkAliveCells = nextGenerationOfCellsArray;

                // Clear the grid for each of the cells...
                for (int i = 0; i < storeAllCells.GetLength(0); i++)
                {
                    grid.UpdateCell(storeAllCells[i, 0], storeAllCells[i, 1], CellState.Blank);
                }

                /*------------------------------------------------------------
                                       IF GHOST MODE IS ON,
                       SHOW THE PAST GENERATIONS FADING AS THEY GET OLDER
                 ------------------------------------------------------------*/
                if (ghostOn)
                {
                    if (ghostMemoryUsed >= 1)
                    {
                        for (int cell = 0; cell < ghostMemories[0].Count; cell++)
                        {
                            grid.UpdateCell(ghostMemories[0][cell][0], ghostMemories[0][cell][1], CellState.Dark);
                        }

                        if (ghostMemoryUsed >= 2)
                        {
                            for (int cell = 0; cell < ghostMemories[1].Count; cell++)
                            {
                                grid.UpdateCell(ghostMemories[1][cell][0], ghostMemories[1][cell][1], CellState.Medium);
                            }

                            if (ghostMemoryUsed >= 3)
                            {
                                for (int cell = 0; cell < ghostMemories[2].Count; cell++)
                                {
                                    grid.UpdateCell(ghostMemories[2][cell][0], ghostMemories[2][cell][1], CellState.Light);
                                }
                            }
                        }
                    }
                }

                /*------------------------------------------------------------
                  FINALLY UPDATE THE GRID WITH THE NEXT GENERATION OF CELLS
                 ------------------------------------------------------------*/
                // Render updates to the console window...
                grid.Render();

                // For each of the cells, update the grid with a new cell...
                for (int i = 0; i < nextGenerationOfCellsArray.GetLength(0); i++)
                {
                    grid.UpdateCell(nextGenerationOfCellsArray[i, 0], nextGenerationOfCellsArray[i, 1], CellState.Full);
                }

                // Render updates to the console window...
                grid.Render();
            }

            /*------------------------------------------------------------
                          AFTER ALL GENERATIONS HAVE PASSED
             ------------------------------------------------------------*/
            // Set complete marker as true and render updates to the terminal (grid now displays COMPLETE)
            grid.IsComplete = true;
            grid.Render();

            // Wait for user to press the spacebar key...
            while (Console.ReadKey().Key != ConsoleKey.Spacebar){}
            
            // Revert grid window size and buffer to the normal terminal
            grid.RevertWindow();

            // If the output option was turned on successfully,
            // print the last generation to the user's .seed file
            string lineBeginning = "(o) cell : ";
            List<string> lines = new List<string>() { "#version=2.0" };

            for (int cell = 0; cell < checkAliveCells.Length / 2; cell++)
            {
                string temporaryLine = lineBeginning;
                
                temporaryLine = temporaryLine + checkAliveCells[cell, 0].ToString() + " ";
                temporaryLine = temporaryLine + checkAliveCells[cell, 1].ToString() + " ";

                lines.Add(temporaryLine);
            }

            if (outputSuccess)
            {
                File.WriteAllLines(outputPathString, lines);
            }

            // Steady-state feature not working properly :(
            if (!steadyStateDetected)
            {
                Console.WriteLine(" Stead-state not detected...");
            }
            else
            {
                Console.WriteLine(" Stead-state detected...");
            }

            if (outputSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" Success: Final generation written to file: {outputPathString}");
                Console.ResetColor();
            }

            // Wait for user to press the spacebar key...
            Console.WriteLine(" Press spacebar to close the program...");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar){}
        }
    }
}
