using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Life
{
    /// <summary>
    /// Used by ParentSettings super class & SubSettings class
    /// Series of checks that determines the success or failure of parameters
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    class Check
    {
        /*-----------------------------------------------------------------
                           CHECK IF PARAM IS ANOTHER ARGUMENT
         -----------------------------------------------------------------*/
        /// <summary>
        /// Checks if a parameter string is another argument
        /// </summary>
        /// <param name="parameters">List of parameter strings</param>
        /// <param name="missingString">The missing parameter message</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the parameter starts with 2 dashes, meaning it's another
        /// argument
        /// </exception>
        public static void CheckIfArgument(List<string> parameters, string missingString)
        {
            foreach (string parameter in parameters)
            {
                if (parameter.StartsWith("--"))
                {
                    throw new ArgumentException($"Missing the {missingString}");
                }
            }
        }

        /*-----------------------------------------------------------------
                   CHECK IF PARAM IS A WHOLE NUMBER WITHIN THE BOUNDS
         -----------------------------------------------------------------*/
        /// <summary>
        /// Checks if the parameters are whole numbers and within given bounds
        /// </summary>
        /// <param name="parameters">List of parameter strings</param>
        /// <param name="minValue">The minimum bound</param>
        /// <param name="maxValue">The maximum bound</param>
        /// <exception cref="System.FormatException">
        /// Thrown as soon as one of the parameters is not a whole number
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown as soon as one of the parameters is outside the given bounds
        /// </exception>
        /// <returns>A list of converted numbers</returns>
        public static List<int> CheckWholeNumbersAndBounds(List<string> parameters,
            int minValue, int maxValue)
        {
            List<int> convertedParams = new List<int>();

            // Checks if parameter is comprised of whole numbers only
            foreach (string parameter in parameters)
            {
                foreach (char ch in parameter)
                {
                    if (ch < '0' || ch > '9')
                    {
                        throw new FormatException($"Must be whole numbers only, " +
                            $"must be {minValue}...{maxValue} inclusive");
                    }
                }
            }

            // Checks if parameter is within the bounds
            foreach (string parameter in parameters)
            {
                int holdValue;

                if (int.TryParse(parameter, out holdValue))
                {
                    if (!(holdValue >= minValue && holdValue <= maxValue))
                    {
                        throw new ArgumentOutOfRangeException("", $"Out of bounds, " +
                            $"must be {minValue}...{maxValue} inclusive");
                    }
                    else
                    {
                        convertedParams.Add(holdValue);
                    }
                }
            }

            return convertedParams;
        }

        /*-----------------------------------------------------------------
               CHECK IF PARAM IS A DECIMAL NUMBER WITHIN THE BOUNDS
         -----------------------------------------------------------------*/
        /// <summary>
        /// Checks if the parameters are whole numbers and within given bounds
        /// </summary>
        /// <param name="parameters">List of parameter strings</param>
        /// <exception cref="System.FormatException">
        /// Thrown as soon as one of the parameters is not a whole number
        /// or a decimal value
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown as soon as one of the parameters is out of bounds
        /// </exception>
        /// <returns>A decimal value</returns>
        public static double CheckDecimalAndBounds(List<string> parameters)
        {
            double randomMin = 0.0;
            double randomMax = 1.0;
            double randomValue = 0.5;

            // Checks if parameter comprises of only numbers and decimals
            foreach (string parameter in parameters)
            {
                foreach (char ch in parameter)
                {
                    if ((ch < '0' || ch > '9') && !(ch == '.'))
                    {
                        throw new FormatException($"Must be decimal numbers only, " +
                            $"{randomMin}...{randomMax} inclusive");
                    }
                }
            }

            // Checks if parameter is within the bounds
            foreach (string parameter in parameters)
            {
                if (double.TryParse(parameter, out randomValue))
                {
                    if (!(randomValue >= randomMin && randomValue <= randomMax))
                    {

                        throw new ArgumentOutOfRangeException("", $"Out of bounds, must be " +
                            $"{randomMin}...{randomMax} inclusive");
                    }
                }
            }

            return randomValue;
        }

        /*-----------------------------------------------------------------
                                CHECK IF FILE EXISTS
         -----------------------------------------------------------------*/
        /// <summary>
        /// Checks if a file exists
        /// </summary>
        /// <param name="path">File path</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the file does not exist or if the existing file is not a
        /// .seed file
        /// </exception>
        /// <returns>A bool that's true if the the file exists</returns>
        public static bool CheckFileExists(string path)
        {
            bool fileExists = false;

            if (!File.Exists(path))
            {
                throw new ArgumentException($"File \'{path}\' does not exist.");
            }
            if (!Path.GetExtension(path).Equals(".seed"))
            {
                throw new ArgumentException($"Extension must be '.seed' not \'{Path.GetExtension(path)}\'");
            }

            fileExists = true;
            return fileExists;
        }

        /*-----------------------------------------------------------------
                          CHECK IF RULES ARE VALID & ASSIGN
         -----------------------------------------------------------------*/
        /// <summary>
        /// Checks if the rules are valid, then assigns them
        /// </summary>
        /// <param name="args">User's inputs</param>
        /// <param name="index">Index of the rule argument within args</param>
        /// <param name="type">Type of rule, birth or survival</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if there are no birth or survival rules, or when the rules
        /// are not less than or equal to the number of neighbouring cells
        /// </exception>
        /// <returns>A list of rules</returns>
        public static List<int> checkRules(string[] args, int index, string type)
        {
            List<int> temporaryRules = new List<int>();

            // Pattern explained:
            // ^        Start of string
            // [0-9 ]+  One or more digits, whitespaces allowed
            // $        End of string
            string isDigitPattern = @"^[0-9 ]+$";

            // Pattern explained
            // ^        Start of string
            // [0-9 ]+  One or more digits, whitespaces allowed
            // \.\.\.   3 dots in a row
            // [0-9 ]+  One or more digits, whitespaces allowed
            // $        End of string
            string isSeperatedDigitsPattern = @"^[0-9 ]+\.\.\.[0-9 ]+$";
            int numberOfNeighbouringCells = (int)Math.Pow((Universe.order * 2) + 1, 2);

            for (int param = index + 1; param < args.Length; param++)
            {
                if (Regex.IsMatch(args[param], isDigitPattern) || Regex.IsMatch(args[param], isSeperatedDigitsPattern))
                {
                    // If the rule matches the digit pattern, it is a whole number only
                    // add this to the list of temporary
                    if (Regex.IsMatch(args[param], isDigitPattern))
                    {
                        if (Int32.TryParse(args[param], out int singleRuleInt))
                        {
                            temporaryRules.Add(singleRuleInt);
                        }
                    }

                    // If the rule matches the seperated digits pattern, it is a range between 2 numbers
                    // Grab all numbers between the 2 rules inclusive and add them all to the temporary rules list
                    if (Regex.IsMatch(args[param], isSeperatedDigitsPattern))
                    {
                        string beforedotdotdot = args[param].Substring(0, args[param].IndexOf('.'));
                        string afterdotdotdot = args[param].Substring(args[param].LastIndexOf('.') + 1);
                        int beforedotdotdotInt = 0;
                        int afterdotdotdotInt = 0;
                        int largestDotInt = 0;
                        int smallestDotInt = 0;

                        // Find the smallest number in the range so
                        // we can count upwards to the largest number
                        if (Int32.TryParse(beforedotdotdot, out beforedotdotdotInt) && Int32.TryParse(afterdotdotdot, out afterdotdotdotInt))
                        {
                            if (beforedotdotdotInt <= afterdotdotdotInt)
                            {
                                smallestDotInt = beforedotdotdotInt;
                                largestDotInt = afterdotdotdotInt;
                            }
                            else
                            {
                                smallestDotInt = afterdotdotdotInt;
                                largestDotInt = beforedotdotdotInt;
                            }

                            for (int smallest = smallestDotInt; smallest <= largestDotInt; smallest++)
                            {
                                temporaryRules.Add(smallest);
                            }
                        }
                        else
                        {
                            break;
                        }       
                    }

                    // Update the runtime display message
                    if (type.IndexOf("birth", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        SubSettings.holdBirth = SubSettings.holdBirth + " " + args[param];
                    }
                    else
                    {
                        SubSettings.holdSurvival = SubSettings.holdSurvival + " " + args[param];
                    }
                }
                else
                {
                    break;
                }
            }

            // Throw exception if there is not at least one rule
            if (!temporaryRules.Any())
            {
                throw new ArgumentException($"Must have at least one survival & birth rule that's a positive whole number or a range between 2 numbers like 1...9");
            }

            // If the neighbourhood is VonNuemann
            // Update the number of neighbouring cells
            if (Universe.von)
            {
                numberOfNeighbouringCells = (int)Math.Pow(Universe.order, 2) + ((int)Math.Pow(Universe.order + 1, 2));
            }

            // If the centre cell is not counted
            // Remove 1 from the number of neighbouring cells
            if (!Universe.countCentre)
            {
                numberOfNeighbouringCells--;
            }

            // Check each rule in temporary rules is less than or equal to the number of neighbouring cells
            // If it's not, throw an exception also showing the user the current number of neighbouring cells
            foreach (int rule in temporaryRules)
            {
                if (!(rule <= numberOfNeighbouringCells))
                {
                    throw new ArgumentException($"Each {type} rule must be less than or equal to the number of neighbouring cells, which is currently {numberOfNeighbouringCells}");
                }  
            }

            return temporaryRules;
        }
    }
}