using System;
using System.Collections.Generic;

namespace Life
{
    /// <summary>
    /// Main class that branches off to other classes' methods
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    public class SubNeighbourhood
    {
        /// <summary>
        /// 1st Checks how many neighbours in the neighbourhood by creating an
        /// instance of the parent neighbourhood
        /// 2nd If the grid is periodic, wrap around the grid to find those
        /// neighbours that might have been out of bounds
        /// 3rd Count how many of the neighbours are alive and return this count
        /// </summary>
        /// <param name="checkAllCells">All possible cells</param>
        /// <param name="checkAliveCells">All currently alive cells</param>
        /// <param name="xValue">Host cell's x coordinate</param>
        /// <param name="yValue">Host cell's y coordinate</param>
        /// <param name="rows">The row count</param>
        /// <param name="cols">The column count</param>
        /// <param name="order">How far the neighbourhood spreads</param>
        /// <param name="von">A bool that's true if the instance is a VonNuemann</param>
        /// <param name="countCentre">A bool that's true if the centre cell is included</param>
        /// <param name="periodicOn">A bool that's true if the neighbours are meant to wrap
        /// around the grid</param>
        /// <returns>The count of alive neighbour cells</returns>
        public static int checkNeighbours(int[,] checkAllCells, int[,] checkAliveCells, int xValue,
            int yValue, int rows, int cols, int order, bool von, bool countCentre, bool periodicOn)
        {
            /*-----------------------------------------------------------------
                                     FIND ALIVE NEIGHBOURS
             -----------------------------------------------------------------*/
            // Initial count of live neighbours is 0
            int aliveCount = 0;

            // Fill neighbours list up by creating an instance
            // Store the amount of neighbours
            List<List<int>> neighbours = new List<List<int>>();

            ParentNeighbourhood findNeighbours = new ParentNeighbourhood(order, xValue, yValue, countCentre, von);
            neighbours = findNeighbours.neighbours;

            int amountOfNeighbours = findNeighbours.neighbours.Count;

            /*-----------------------------------------------------------------
                                     PERIODIC BEHAVIOUR
             -----------------------------------------------------------------*/
            // If periodic setting is on, check if the neighbour cells exist...
            // and if they do not, wrap around the grid using modulus
            if (periodicOn)
            {
                // Cycle through every neighbour
                for (int allCell = 0; allCell < amountOfNeighbours; allCell++)
                {
                    neighbours[allCell][0] = (neighbours[allCell][0] + rows) % rows;
                    neighbours[allCell][1] = (neighbours[allCell][1] + cols) % cols;
                }
            }

            /*-----------------------------------------------------------------
                                    COUNT LIVE NEIGHBOURS
             -----------------------------------------------------------------*/
            int lengthOfAlive = checkAliveCells.Length / 2;

            for (int allCell = 0; allCell < amountOfNeighbours; allCell++)
            {

                for (int aliveCell = 0; aliveCell < lengthOfAlive; aliveCell++)
                {
                    if (neighbours[allCell][0] == checkAliveCells[aliveCell, 0] &&
                        neighbours[allCell][1] == checkAliveCells[aliveCell, 1])
                    {
                        aliveCount++;
                        break;
                    }
                }
            }

            // Return the count
            return aliveCount;
        }
    }
}
