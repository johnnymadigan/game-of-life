using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Life
{
    /// <summary>
    /// Super parent settings class that can be re-used for all settings
    /// Interprets command line arguments and validates them using a series of checks
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    class ParentSettings
    {
        public string yesOrNo = "No       ";
        public string message = "Defaults";
        public List<string> parameters = new List<string>();
        public List<int> convertedParams = new List<int>();
        public double randomFactor = 0.5;
        public int argIndex = 0;

        /// <summary>
        /// By creating an instance with specific values...
        /// the instance will validate unique parameters
        /// </summary>
        /// <param name="args">The user's inputs</param>
        /// <param name="checkWholeNumbersAndBounds">Whether or not to check if
        /// the param is a whole number and within the given bounds</param>
        /// <param name="yes">Whether or not to switch the parameter on</param>
        /// <param name="checkRandom">Whether or not to check if the param is a
        /// whole number or a decimal between 0...1</param>
        /// <param name="checkDirectories">Whether or not to check if a directory exists</param>
        /// <param name="checkFiles">Whether or not to check if a file exists</param>
        /// <param name="arg">The specific argument to find</param>
        /// <param name="missingString">What to show the user if the param is missing</param>
        /// <param name="howManyParams">The number of params needed to be found</param>
        /// <param name="minValue">The minimum given bound</param>
        /// <param name="maxValue">The maximum given bound</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the parameters for the argument are missing
        /// </exception>
        public ParentSettings(string[] args, bool checkWholeNumbersAndBounds, bool yes,
            bool checkRandom, bool checkDirectories, bool checkFiles, string arg, string missingString,
            int howManyParams = 0, int minValue = 0, int maxValue = 999999999)
        {
            try
            {
                /*--------------------------------------------------------------
                             IF ARGUMENT EXISTS IN USER'S INPUTS
                 --------------------------------------------------------------*/
                if (args.Contains(arg, StringComparer.OrdinalIgnoreCase))
                {
                    // Grab index of the case insensitive argument if it exists
                    argIndex = Array.FindIndex(args, t => t.IndexOf(arg,
                        StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (args.Length >= (argIndex + (howManyParams + 1)))
                    {
                        for (int currentParam = 1; currentParam <= howManyParams; currentParam++)
                        {
                            parameters.Add(args[argIndex + currentParam]);
                        }

                        /*------------------------------------------------------
                               CHECK THE SUCCESS OR FAILURE OF PARAMETERS
                         ------------------------------------------------------*/
                        Check.CheckIfArgument(parameters, missingString);

                        if (checkWholeNumbersAndBounds)
                        {
                            convertedParams = (Check.CheckWholeNumbersAndBounds(parameters,
                                minValue, maxValue));
                        }

                        if (yes)
                        {
                            yesOrNo = "Yes      ";
                        }

                        if (checkRandom)
                        {
                            randomFactor = (Check.CheckDecimalAndBounds(parameters));
                        }

                        // If all the above checks pass, argument was a success
                        message = "Success!";

                        if (checkFiles)
                        {
                            Check.CheckFileExists(parameters[0]);

                            // If the file exists, report back with an empty message
                            // This will prompt the seed to be converted to integers
                            // If the converted seed is within the bounds of the grid,
                            // the seed will be planted
                            message = "";
                        }
                    }

                    else
                    {
                        throw new ArgumentException($"Missing the {missingString}");
                    }
                }
            }
            /*------------------------------------------
                         CATCH ANY EXCEPTIONS
             ------------------------------------------*/
            catch (FormatException ex)
            {
                message = ex.Message;
                SubSettings.foundAnException = true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                message = ex.Message;
                SubSettings.foundAnException = true;
            }
            catch (ArgumentException ex)
            {
                message = ex.Message;
                SubSettings.foundAnException = true;
            }
            catch (DirectoryNotFoundException ex)
            {
                message = ex.Message;
                SubSettings.foundAnException = true;
            }
            catch (FileNotFoundException ex)
            {
                message = ex.Message;
                SubSettings.foundAnException = true;
            }
        }
    }
}
