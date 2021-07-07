/*
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
*/

namespace Life
{
    /// <summary>
    /// Main class that branches off to other classes
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    class Program
    {
        /// <summary>
        /// First reads arguments, updates settings and displays them. Then
        /// brings the game to life. SubSettings.Update() can immidiately start
        /// the game of life using default settings (just click run in VS) or
        /// 'dotnet life.dll' in the terminal (once in root>Life>bin>Debug>netcoreapp3.1)
        /// </summary>
        /// <param name="args">User's input to alter of the Game of Life</param>
        static void Main(string[] args)
        {
            /*--------------------------------------------------
                       INTERPRET & DISPLAY ARGUMENTS
             --------------------------------------------------*/
            SubSettings.Update(args);

            /*--------------------------------------------------
                        BRING THE UNIVERSE TO LIFE
             --------------------------------------------------*/
            Universe.BringToLife();
        }
    }
}
