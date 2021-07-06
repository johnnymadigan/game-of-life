using System;
using System.Collections.Generic;

namespace Life
{
    /// <summary>
    /// Initial sets up every possible cell for the universe along with
    /// the initial live cells, whether seed or random
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    public class UniverseSetup
    {
        /*-----------------------------------------------------------------
                             GENERATE ALL POSSIBLE CELLS
         -----------------------------------------------------------------*/
        /// <summary>
        /// Generate all {x,y} coordinates for EVERY cell in the grid
        /// The array returned will be used to check the neighbours of
        /// each cell, and whether the neighbour exists within the grid
        /// Allowing for features such as the periodic behaviour, where
        /// the cell will wrap around if the neighbour is out of bounds
        /// </summary>
        /// <param name="totalCells">The amount of possible cells</param>
        /// <param name="rows">The row count</param>
        /// <param name="columns">The column count</param>
        /// <returns>All cell positions in the grid</returns>
        public static int[,] GenerateAllCells(int totalCells, int rows, int columns)
        {
            // Empty array the size of the totalCells
            int[,] generateAllCells = new int[totalCells, 2];

            // Loop to fill array with all possible cells
            int xValue = 0;

            for (int cellCount = 0; cellCount < totalCells; cellCount++)
            {
                for (int yValue = 0; yValue < columns; yValue++)
                {
                    generateAllCells[cellCount, 0] = xValue;
                    generateAllCells[cellCount, 1] = yValue;
                    cellCount++;
                }

                xValue++;
                cellCount--;
            }

            return generateAllCells;
        }

        /*-----------------------------------------------------------------
                          GENERATE INITIAL RANDOM LIVE CELLS
         -----------------------------------------------------------------*/
        /// <summary>
        /// If there is no seed, randomly generate the live cells by bringing
        /// cells to life based on the random factor
        /// </summary>
        /// <param name="totalCells">The amount of possible cells</param>
        /// <param name="random">The random factor value</param>
        /// <param name="cells">An array holding all possible cells</param>
        /// <returns>The new array specifying only the cells which are alive</returns>
        public static int[,] GenerateRandomCells(int totalCells, double random, int[,] allCells)
        {
            int cellPosition;
            List<List<int>> aliveCellsList = new List<List<int>>();

            // Randomly brings cell to life (default 50% chance of being born)
            for (cellPosition = 0; cellPosition < totalCells; cellPosition++)
            {
                Random bringCellToLife = new Random();
                bool randomBool = bringCellToLife.NextDouble() < random;
                if (randomBool)
                {
                    aliveCellsList.Add(new List<int>() { allCells[cellPosition, 0], allCells[cellPosition, 1] });
                }
            }

            // Converting the list to an array so it can be returned and easily used
            int[,] aliveCellsArray = new int[aliveCellsList.Count, 2];

            for (int i = 0; i < aliveCellsList.Count; i++)
            {
                aliveCellsArray[i, 0] = aliveCellsList[i][0];
                aliveCellsArray[i, 1] = aliveCellsList[i][1];
            }

            return aliveCellsArray;
        }
    }
}
