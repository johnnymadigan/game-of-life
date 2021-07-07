using System;

namespace Life
{
    /// <summary>
    /// Display runtime settings
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    class Runtime
    {
        /*-----------------------------------------------------------------
                        MAIN DISPLAY RUNTIME SETTINGS METHOD
         -----------------------------------------------------------------*/
        /// <summary>
        /// Displat the main messages to the user, then calls other methods
        /// to display the runtime settings
        /// </summary>
        /// <param name="foundAnException">Bool that's true if an exception was found eariler</param>
        /// <param name="foundUnrecognisedOptions">Bool that's true if unrecognised options were found</param>
        /// <param name="unrecognisedOptions">The unrecognised options found, if any</param>
        /// <param name="message">The status of the setting</param>
        /// <param name="argString">The setting's name</param>
        /// <param name="argValue">The setting's value</param>
        public static void DisplayRuntime(bool foundAnException, bool foundUnrecognisedOptions,
            string unrecognisedOptions, string[] messages, string[] argStrings, string[] argValues)
        {
            /// Display GoL titl
            Console.WriteLine("               .---.  .----. .-.   ");
            Console.WriteLine("              /   __}/  {}  \\| |   ");
            Console.WriteLine("              \\  {_ }\\      /| `--.");
            Console.WriteLine("               `---'  `----' `----'");
            Console.WriteLine("               GAME     of    LIFE");

            // Display that there were no errors or that there were errors and
            // reverting back to defaults for those settings with errors
            if (foundAnException || foundUnrecognisedOptions)
            {
                if (foundAnException)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(" Warning: Issue processing some command line arguments, reverting to defaults.");
                    Console.ResetColor();
                }

                if (foundUnrecognisedOptions)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Error: Unrecognised options{0}", unrecognisedOptions);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Success: All command line arguments processed.");
                Console.ResetColor();
            }

            // Display runtime settings + their values (default or custom) + status (Defaults, Success!, error)
            Console.WriteLine(" The program will use the following settings:\n");

            for (int line = 0; line < messages.Length; line++)
            {
                DisplayValue(messages[line], argStrings[line], argValues[line]);
            }
        }

        /*-----------------------------------------------------------------
                              DISPLAY RUNTIME SETTINGS
         -----------------------------------------------------------------*/
        /// <summary>
        /// Displays the setting's name along with it's current value and status
        /// </summary>
        /// <param name="message">The status of the setting</param>
        /// <param name="argString">The setting's name</param>
        /// <param name="argValue">The setting's value</param>
        public static void DisplayValue(string message, string argString, string argValue)
        {
            Console.Write("{0, 22} {1}\t", argString, argValue);
            DisplayMessage(message);
        }

        /*-----------------------------------------------------------------
                             DISPLAY SETTING'S STATUS
         -----------------------------------------------------------------*/
        /// <summary>
        /// Displays the runtime status based on the given message parameter
        /// Status is either "Success!", "Defaults", or a custom error message
        /// </summary>
        /// <param name="message">The current status of the setting</param>
        public static void DisplayMessage(string message)
        {
            if (message.Contains("Success!"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0}\n", message);
                Console.ResetColor();
            }
            else if (message.Contains("Defaults"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("{0}\n", message);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}\n", message);
                Console.ResetColor();
            }   
        }
    }
}
