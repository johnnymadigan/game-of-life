using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Life
{
    /// <summary>
    /// A series of child instances inhereting the super parent settings class
    /// These instances check if the user wants to update the settings and
    /// they've given valid parameters
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    class SubSettings
    {
        /*-----------------------------------------------------------------
                      GLOBAL AND SHARED PRIVATE VARIABLES
                        FOR DISPLAYING RUNTIME SETTINGS
         -----------------------------------------------------------------*/
        // PRIVATE DISPLAY ARGUMENT STRINGS
        private static string seedString = "Input File:";
        private static string outputString = "Output File:";
        private static string generationsString = "Generations:";
        private static string memoryString = "Memory:";
        private static string updatesString = "Refresh Rate:";
        private static string rulesString = "Rules:";
        private static string neighboursString = "Neighbourhood:";
        private static string periodicString = "Periodic:";
        private static string rowsString = "Rows:";
        private static string colsString = "Columns:";
        private static string randomString = "Random Factor:";
        private static string stepString = "Step Mode:";
        private static string ghostString = "Ghost:";

        // PRIVATE DISPLAY PARAMETER STRINGS
        // The huge spaces are used to keep all strings at roughly the same length
        // So when they are displayed they are formatted aesthetically
        public static string seedDisplay = "N/A      ";
        public static string outputDisplay = "N/A      ";
        private static string generationsDisplay = Universe.generationsCount.ToString() + "        ";
        private static string memoryDisplay = Universe.steadyLimit.ToString() + "        ";
        private static string updatesPerSecondDisplay = Universe.updatesPerSecond.ToString() + "        ";
        private static string rulesDisplay = "S( 2 3 ) B( 3 )";
        public static string holdBirth = "";
        public static string holdSurvival = "";
        private static string neighboursDisplay = "Moore (1)";
        private static string rowsDisplay = Universe.rows.ToString() + "        ";
        private static string colsDisplay = Universe.cols.ToString() + "        ";
        private static string randomDisplay = "50%      ";

        // PRIVATE UNRECOGNISED OPTIONS OR FOUND EXCEPTIONS VARIABLES
        private static string unrecognisedOptions = "";
        private static string rulesMessage = "Defaults";
        private static bool foundUnrecognisedOptions = false;
        public static bool foundAnException = false;

        /// <summary>
        /// Interprets user's inputs and updates default settings accordingly
        /// </summary>
        /// <param name="args">User inputs to alter the Game of Life</param>
        public static void Update(string[] args)
        {
            /*-----------------------------------------------------------------
                                     DIMENSIONS ARGUMENT
             -----------------------------------------------------------------*/
            // Instance checking if parameter is a whole number and within bounds
            // of 4...48 inclusive
            ParentSettings dimensions = new ParentSettings(args, true, false, false,
                false, false, "--dimensions", "rows & columns parameters", 2, 4, 48);

            if (dimensions.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Universe.rows = dimensions.convertedParams[0];
                Universe.cols = dimensions.convertedParams[1];
                rowsDisplay = Universe.rows.ToString() + "        ";
                colsDisplay = Universe.cols.ToString() + "        ";
                Universe.totalCells = Universe.rows * Universe.cols;
            }

            /*-----------------------------------------------------------------
                                     PERIODIC ARGUMENT
             -----------------------------------------------------------------*/
            // Instance confirming periodic behaviour
            ParentSettings periodic = new ParentSettings(args, false, true, false,
                false, false, "--periodic", "");

            if (periodic.yesOrNo.Contains("Y"))
            {
                Universe.periodicOn = true;
            }

            /*-----------------------------------------------------------------
                                      OUTPUT ARGUMENT
             -----------------------------------------------------------------*/
            // Instance checking if there's a relative or absolute path to a .seed file
            // If found the last generation will be written to this file
            ParentSettings output = new ParentSettings(args, false, false, false,
                true, true, "--output", "path for output parameter", 1);

            if (string.IsNullOrEmpty(output.message))
            {
                Universe.outputSuccess = true;
                Universe.outputPathString = output.parameters[0];
                outputDisplay = Path.GetFileName(output.parameters[0]);
                if (Path.GetFileName(output.parameters[0]).Length < 15)
                {
                    output.message = "Success!";
                }

            }

            /*-----------------------------------------------------------------
                                   NEIGHBOURS ARGUMENT
             -----------------------------------------------------------------*/
            // Instance checking if there are 3 neighbour parameters
            ParentSettings neighbour = new ParentSettings(args, false, false, false,
                false, false, "--neighbour", "path parameter", 3);

            try
            {
                // If enough parameters were found, validate these parameters
                // It must either be a Moore of VonNuemann neighbourhood
                // It must be less than half of the smallest dimension
                if (neighbour.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (!((neighbour.parameters[0].IndexOf("vonneumann", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (neighbour.parameters[0].IndexOf("moore", StringComparison.OrdinalIgnoreCase) >= 0)))
                    {
                        throw new ArgumentException("Must be a vonNeumann or moore");
                    }

                    List<string> neighbourOrder = new List<string>();
                    List<int> neighbourOrderInt = new List<int>();
                    neighbourOrder.Add(neighbour.parameters[1]);

                    neighbourOrderInt = (Check.CheckWholeNumbersAndBounds(neighbourOrder, 1, 10));

                    int smallestDimension = 16;

                    if (Universe.rows <= Universe.cols)
                    {
                        smallestDimension = Universe.rows;
                    }
                    else
                    {
                        smallestDimension = Universe.cols;
                    }

                    if (!(neighbourOrderInt[0] < smallestDimension / 2))
                    {
                        throw new ArgumentOutOfRangeException("",
                            "Order must be less than half of the smallest dimension");
                    }


                    if (!((neighbour.parameters[2].IndexOf("true", StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (neighbour.parameters[2].IndexOf("false", StringComparison.OrdinalIgnoreCase) >= 0)))
                    {
                        throw new ArgumentException("Must be true or false");
                    }

                    // If no exceptions are thrown
                    // Toggle whether it is a VonNuemann or not & whether to count the centre cell or not
                    if (neighbour.parameters[0].IndexOf("vonneumann", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Universe.von = true;
                        neighboursDisplay = "VonNeumann (" + neighbourOrderInt[0].ToString() + ")";
                    }
                    else
                    {
                        neighboursDisplay = "Moore (" + neighbourOrderInt[0].ToString() + ")";
                    }

                    Universe.order = neighbourOrderInt[0];

                    if (neighbour.parameters[2].IndexOf("true", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Universe.countCentre = true;
                    }
                }
            }
            catch (FormatException ex)
            {
                neighbour.message = ex.Message;
                foundAnException = true;
            }
            
            catch (ArgumentOutOfRangeException ex)
            {
                neighbour.message = ex.Message;
                foundAnException = true;
            }
            catch (ArgumentException ex)
            {
                neighbour.message = ex.Message;
                foundAnException = true;
            }

            /*-----------------------------------------------------------------
                                      RULES ARGUMENT
             -----------------------------------------------------------------*/
            // Instances to grab the index of birth and survival arguments
            ParentSettings survival = new ParentSettings(args, false, false, false,
                false, false, "--survival", "rules parameters", 1);

            ParentSettings birth = new ParentSettings(args, false, false, false,
                false, false, "--birth", "rules parameters", 1);

            try
            {
                // If both rules have at least one parameter, validate these parameters
                // Parameters must be whole numbers, or a range between 2 numbers like 1...9
                if (birth.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0 &&
                    survival.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Universe.survival = Check.checkRules(args, survival.argIndex, "survival");
                    Universe.birth = Check.checkRules(args, birth.argIndex, "birth");

                    rulesDisplay = "S(" + holdSurvival + " ) B(" + holdBirth + " )";

                    rulesMessage = birth.message;

                    if (rulesDisplay.Length > 15)
                    {
                        rulesMessage = "";
                    }
                }

                // If there's not even one parameter for one or both of the rules, tell the user
                else
                {
                    if (((birth.message.IndexOf("missing", StringComparison.OrdinalIgnoreCase) >= 0)) &&
                        ((survival.message.IndexOf("missing", StringComparison.OrdinalIgnoreCase) >= 0)))
                    {
                        rulesMessage = "Missing both birth and survival parameters";
                    }
                    else if (!(birth.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        rulesMessage = birth.message;
                    }
                    else
                    {
                        rulesMessage = survival.message;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                rulesMessage = ex.Message;
            }

            /*-----------------------------------------------------------------
                                        SEED ARGUMENT
             -----------------------------------------------------------------*/
            // Instance checking if there's a relative or absolute path to a .seed file
            // If found the seed's contents will be validated
            // If valid the seed will be planted
            ParentSettings seed = new ParentSettings(args, false, false, false,
                true, true, "--seed", "path to file parameter", 1);

            if (string.IsNullOrEmpty(seed.message))
            {
                try
                {
                    Universe.storeSeedCells = Seed.PlantSeed(Seed.CheckFilesInsides(seed.parameters[0]),
                        seed.parameters[0]);
                    Universe.seedSuccess = true;
                    if (Path.GetFileName(seed.parameters[0]).Length < 15)
                    {
                        seed.message = "Success!";
                    }
                }
                catch (FormatException ex)
                {
                    seed.message = ex.Message;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    seed.message = ex.Message;
                }
                catch (ArgumentException ex)
                {
                    seed.message = ex.Message;
                }
            }

            /*-----------------------------------------------------------------
                                      RANDOM ARGUMENT
             -----------------------------------------------------------------*/
            // Instance checking the random factor is compirsed of digits, decimals
            // and within the bounds of 0...1 inclusive
            ParentSettings random = new ParentSettings(args, false, false, true,
                false, false, "--random", "random factor parameter", 1);

            if (!(Universe.seedSuccess))
            {
                if (random.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Universe.randomFactor = random.randomFactor;
                    double randomFormatted = Universe.randomFactor * 100;
                    randomDisplay = String.Format("{0:0}%      ", randomFormatted);
                }
            }
            else
            {
                random.message = "Disabled by seed";
                randomDisplay = "N/A      ";
            }

            /*-----------------------------------------------------------------
                                   GENERATIONS ARGUMENT
             -----------------------------------------------------------------*/
            // Instance checking if the parameter is a whole number above 0
            ParentSettings generations = new ParentSettings(args, true, false, false,
                false, false, "--generations", "generations parameter", 1, 1);

            if (generations.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Universe.generationsCount = generations.convertedParams[0];
                generationsDisplay = Universe.generationsCount.ToString() + "        ";
            }

            /*-----------------------------------------------------------------
                                    STEP MODE ARGUMENT
             -----------------------------------------------------------------*/
            // Instance confirming step mode
            ParentSettings step = new ParentSettings(args, false, true, false,
                false, false, "--step", "");

            if (step.yesOrNo.Contains("Y"))
            {
                Universe.stepModeOn = true;
            }

            /*-----------------------------------------------------------------
                                MAXIMUM UPDATE RATE ARGUMENT
             -----------------------------------------------------------------*/
            // Instance checking if the parameter is a whole number within bounds
            // of 1...30 inclusive
            ParentSettings updates = new ParentSettings(args, true, false, false,
                false, false, "--max-update", "refresh rate parameter", 1, 1, 30);

            if (!Universe.stepModeOn)
            {
                if (updates.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Universe.updatesPerSecond = updates.convertedParams[0];
                    updatesPerSecondDisplay = Universe.updatesPerSecond.ToString() + "        ";
                }
            }
            else
            {
                updates.message = "Disabled by step mode";
                updatesPerSecondDisplay = "N/A      ";
            }

            /*-----------------------------------------------------------------
                                       MEMORY ARGUMENT
             -----------------------------------------------------------------*/
            // Instance checking if the parameter is a whole number
            // and between 4...512 inclusive
            ParentSettings memory = new ParentSettings(args, true, false, false,
                false, false, "--memory", "memory parameter", 1, 4, 512);

            if (memory.message.IndexOf("success", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Universe.steadyLimit = memory.convertedParams[0];
                memoryDisplay = Universe.steadyLimit.ToString() + "        ";
            }

            /*-----------------------------------------------------------------
                                        GHOST ARGUMENT
             -----------------------------------------------------------------*/
            // Instance confirming ghost mode
            ParentSettings ghost = new ParentSettings(args, false, true, false,
                false, false, "--ghost", "");

            if (ghost.yesOrNo.Contains("Y"))
            {
                Universe.ghostOn = true;
            }

            /*-----------------------------------------------------------------
                             FIND REMAINING UNRECOGNISED OPTIONS
             -----------------------------------------------------------------*/
            string[] possibleCommands = { "--dimensions", "--neighbour", "--periodic",
                "--memory", "--random", "--seed", "--output", "--generations", "--max-update",
                "--step", "--birth", "--survival", "--ghost"};

            foreach (string option in args)
            {
                // If option is not one of the possible commands
                // Add the unrecognised option to the end of the string
                // This string will be displayed later with all the unrecognised options
                if (option.Contains("--"))
                {
                    if (!(possibleCommands.Contains(option, StringComparer.OrdinalIgnoreCase)))
                    {
                        unrecognisedOptions = unrecognisedOptions + " " + option;
                        foundUnrecognisedOptions = true;
                    }
                }
            }

            /*-----------------------------------------------------------------
                                    DISPLAY RUNTIME SETTINGS
             -----------------------------------------------------------------*/
            string[] messages = {seed.message, output.message, generations.message, memory.message,
                updates.message, rulesMessage, neighbour.message, periodic.message, dimensions.message,
                dimensions.message, random.message, step.message, ghost.message};

            string[] argStrings = { seedString, outputString, generationsString, memoryString,
                updatesString, rulesString, neighboursString, periodicString, rowsString, colsString,
                randomString, stepString, ghostString};

            string[] argValues = {seedDisplay, outputDisplay, generationsDisplay, memoryDisplay,
                updatesPerSecondDisplay, rulesDisplay, neighboursDisplay, periodic.yesOrNo, rowsDisplay,
                colsDisplay, randomDisplay, step.yesOrNo, ghost.yesOrNo};

            Runtime.DisplayRuntime(foundAnException, foundUnrecognisedOptions,
                unrecognisedOptions, messages, argStrings, argValues);
        }
    }
}
