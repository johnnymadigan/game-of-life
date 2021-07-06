using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Life
{
    /// <summary>
    /// Seed class that processes anything seed related
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    class Seed
    {
        /*-----------------------------------------------------------------
                               SHARED PRIVATE VARIABLE
         -----------------------------------------------------------------*/
        private static List<List<int>> convertedSeedCells = new List<List<int>>();
        private static List<List<int>> convertedDeadCells = new List<List<int>>();


        /*-----------------------------------------------------------------
                               CHECK SEED's INSIDES
                             VERSION 1 & 2 COMPATIBLE
         -----------------------------------------------------------------*/
        /// <summary>
        /// Checks the seed's contents are within the bounds, whole numbers,
        /// skipping null lines, and generating all cell coordinates for
        /// rectangles and ellipses
        /// </summary>
        /// <param name="file">The file path</param>
        /// <exception cref="System.FormatException">
        /// Thrown as soon as one of the seed's coordinates is not a whole number,
        /// white spaces or commas
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when a rectangle or ellipse does not have 4 coordinates
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown if a seed cell is larger than any possible grid or if it's
        /// larger than the current valid grid, recommend the grid size that would
        /// suit the seed
        /// </exception>
        /// <returns>A bool that's true if the fild's insides are valid</returns>
        public static bool CheckFilesInsides(string file)
        {
            string[] seedLines = { "empty" };
            bool seedInBounds = true;
            int maxSeedRow = 0;
            int maxSeedCol = 0;
            int cellCordsNeeded = 2;
            int rectangleAndEllipseCordsNeeded = 4;
            string cellCordsMissingMessage = "Each cell needs 2 coordinates";
            string rectangleOrEllipseCordsMissingMessage = "Must have 4 coordinates, " +
                "2 for the bottom left cell and 2 for the top right";

            // Read file line by line into a string array 
            // Always skipping line 1 as it just has the version number
            // And always skipping null lines
            seedLines = File.ReadAllLines(file).Skip(1).Where(item => !String.IsNullOrWhiteSpace(item)).ToArray();

            // For every line:
            // Check if it only comprises of numbers, white spaces and commas
            // Match and convert these coordinates from strings into an int array
            for (int line = 0; line < seedLines.Length; line++)
            {  
                int firstNumIndex = seedLines[line].IndexOfAny("0123456789".ToCharArray());
                string seedSubstring = seedLines[line].Substring(firstNumIndex);

                foreach (char ch in seedSubstring)
                {
                    if ((ch < '0' || ch > '9') && !(ch == ' ') && !(ch == ','))
                    {
                        throw new FormatException("Coordinates must be whole numbers only");
                    }
                }

                int[] convertedCords = Regex.Matches(seedLines[line], @"\d+")
                    .OfType<Match>()
                    .Select(e => int
                    .Parse(e.Value))
                    .ToArray();

                List<int> convertedCordsList = new List<int>();

                // If the seed has an ellipse or a rectangle, make sure there are 4 coordinates
                // 2 for the bottom left cell and 2 for the top right cell, put these into a list
                if ((seedLines[line].IndexOf("ellipse", StringComparison.OrdinalIgnoreCase) >= 0) ||
                        seedLines[line].IndexOf("rectangle", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (!(convertedCords.Length >= rectangleAndEllipseCordsNeeded))
                    {
                        throw new ArgumentException(rectangleOrEllipseCordsMissingMessage);
                    }

                    for (int i = 0; i < rectangleAndEllipseCordsNeeded; i++)
                    {
                        convertedCordsList.Add(convertedCords[i]);
                    }

                    int topX = 0;
                    int topY = 0;
                    int bottomX = 0;
                    int bottomY = 0;

                    // Assign the pair furthest away from {0,0} as the top right coordinates
                    // And assign the pair closest to {0,0} as the bottom left coordinates
                    if (convertedCordsList[2] >= convertedCordsList[0] &&
                        convertedCordsList[3] >= convertedCordsList[1])
                    {
                        topX = convertedCordsList[2];
                        topY = convertedCordsList[3];
                        bottomX = convertedCordsList[0];
                        bottomY = convertedCordsList[1];
                    }
                    else if (convertedCordsList[0] >= convertedCordsList[2] &&
                        convertedCordsList[1] >= convertedCordsList[3])
                    {
                        topX = convertedCordsList[0];
                        topY = convertedCordsList[1];
                        bottomX = convertedCordsList[2];
                        bottomY = convertedCordsList[3];
                    }

                    double width = (topX - bottomX) + 1;
                    double height = (topY - bottomY) + 1;
                    double centreX = bottomX + ((double)(topX - bottomX) / 2);
                    double centreY = bottomY + ((double)(topY - bottomY) / 2);

                    // Go from row to row, column to column, grabbing each cell
                    // in between the bottom left and top right
                    // This will create a rectangle or an ellipse's bound
                    for (int row = bottomX; row <= topX; row++)
                    {
                        for (int col = bottomY; col <= topY; col++)
                        {
                            // If the line specifies an ellipse
                            // Use the equation to determine if the cell is within the ellipse's bounds
                            // Then determine whether it goes into the dead cell's list if specified
                            // by (x), otherwise it is an alive cell
                            if (seedLines[line].IndexOf("ellipse", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                if ((((4 * Math.Pow((row - centreX), 2)) / Math.Pow(width, 2)) +
                                    ((4 * Math.Pow((col - centreY), 2)) / Math.Pow(height, 2))) <= 1)
                                {
                                    if (seedLines[line].Contains("(x)"))
                                    {
                                        convertedDeadCells.Add(new List<int>() { row, col });
                                    }
                                    else
                                    {
                                        convertedSeedCells.Add(new List<int>() { row, col });
                                    }
                                }
                            }
                            // Determine whether it goes into the dead cell's list if specified
                            // by (x), otherwise it is an alive cell
                            else
                            {
                                if (seedLines[line].Contains("(x)"))
                                {
                                    convertedDeadCells.Add(new List<int>() { row, col });

                                }
                                else
                                {
                                    convertedSeedCells.Add(new List<int>() { row, col });
                                }
                            }     
                        }
                    }
                }

                // If the line specifies a single cell
                // Determine whether it goes into the dead cell's list if specified
                // by (x), otherwise it is an alive cell
                else
                {
                    if (!(convertedCords.Length >= cellCordsNeeded))
                    {
                        throw new ArgumentException(cellCordsMissingMessage);
                    }

                    for (int i = 0; i < cellCordsNeeded; i++)
                    {
                        convertedCordsList.Add(convertedCords[i]);
                    }

                    int x = convertedCordsList[0];
                    int y = convertedCordsList[1];

                    if (seedLines[line].Contains("(x)"))
                    {
                        convertedDeadCells.Add(new List<int>() { x, y });
                    }
                    else
                    {
                        convertedSeedCells.Add(new List<int>() { x, y });
                    }

                }

                // Cross check the dead cells with the alive cells
                // If a cell is meant to be dead, remove it from the alive cells list
                foreach (List<int> list in convertedDeadCells)
                {
                    convertedSeedCells.RemoveAll(innerList => innerList.SequenceEqual(list));
                }

                // Reset the dead cells list to avoid errors
                convertedDeadCells = new List<List<int>>();

                // Check to see if the seed is within the bounds
                for (int cell = 0; cell < convertedSeedCells.Count; cell++)
                {
                    if (convertedSeedCells[cell][0] > maxSeedRow)
                    {
                        maxSeedRow = convertedSeedCells[cell][0];
                    }

                    if (convertedSeedCells[cell][1] > maxSeedCol)
                    {
                        maxSeedCol = convertedSeedCells[cell][1];
                    }

                    if (!(((maxSeedRow < Universe.rows && maxSeedRow >= 0) &&
                        (maxSeedCol < Universe.cols && maxSeedCol >= 0))))
                    {
                        seedInBounds = false;
                    }
                }
            }

            if (!seedInBounds)
            {
                // If the seed was out of bounds, recommend the minimum rows
                // rows & columns to accomodate the seed
                // Increment by 1 because grid starts at {0,0} not {1,1} so
                // the recommended grid can accomodate all of the seed's cells 
                maxSeedRow++;
                maxSeedCol++;

                if (maxSeedCol > 48)
                {
                    throw new ArgumentOutOfRangeException("", "Seed is outside the largest possible grid, " +
                        "make sure each cell coordinate is under 48");
                }

                if (maxSeedRow > 48)
                {
                    throw new ArgumentOutOfRangeException("", "Seed is outside the largest possible grid, " +
                        "make sure each cell coordinate is under 48");
                }

                throw new ArgumentOutOfRangeException("", $"A seed cell is out of bounds, " +
                    $"recommending grid size of at least {maxSeedRow} by {maxSeedCol}");
            }

            return seedInBounds;
        }

        /*-----------------------------------------------------------------
                                    PLANT THE SEED
                               VERSION 1 & 2 COMPATIBLE
         -----------------------------------------------------------------*/
        /// <summary>
        /// Checks the seed's contents are within the bounds, whole numbers,
        /// skipping null lines, and generating all cell coordinates for
        /// rectangles and ellipses
        /// </summary>
        /// <param name="file">The file path</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the parameter starts with 2 dashes, meaning it's another
        /// argument
        /// </exception>
        /// <returns>An array holding the seed's cell's coordinates</returns>
        public static int[,] PlantSeed(bool seedInBounds, string file)
        {
            int[,] storeSeedCells = { { 0, 0 } };

            if (seedInBounds)
            {
                storeSeedCells = new int[convertedSeedCells.Count, 2];

                // Put seed's cells into a multi-dimensional int array
                // Returning this array so it can be used to plant the seed
                for (int i = 0; i < convertedSeedCells.Count; i++)
                {
                    storeSeedCells[i, 0] = convertedSeedCells[i][0];
                    storeSeedCells[i, 1] = convertedSeedCells[i][1];
                }

                // Show the user the file name
                SubSettings.seedDisplay = Path.GetFileName(file);
            }

            return storeSeedCells;
        }

        /*-----------------------------------------------------------------
                                   OUTPUT A SEED
                                 IN VERSION 2 FORM
         -----------------------------------------------------------------*/
        /// <summary>
        /// Writes over all lines in the file with new given lines
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="lines">An array of strings to be printed</param>
        public static void OutputSeed(string path, string[] lines)
        {
            File.WriteAllLines(path, lines);
        }
    }
}