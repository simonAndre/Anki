using System;
using System.Linq;

namespace Anki
{


    public class Helpers
    {

        public static void ClearDisplay()
        {
            Console.Clear();
        }

        /// <summary>
        /// get the user choice for a box
        /// </summary>
        /// <returns></returns>
        public static Box ReadBoxKey()
        {
            while (true)
            {
                Console.Write(">");
                // git ascii code for hit key
                var key = (int)Console.ReadKey().KeyChar;
                if (key >= 49 && key <= 51)
                {
                    // ascii chars 49 to 51 are the digit 1 to 3 -> we can convert them to the Box Enum value
                    return (Box)(key - 48);
                }
                Console.WriteLine("please enter either 1 for the green box, 2 for the orange box, 3 for the red box");
            }
        }
        public static void Pause(string nextActionInfo = "")
        {
            Display($"please, hit a key {nextActionInfo}", ConsoleColor.Gray);
            Console.ReadKey(true);
        }

        /// <summary>
        /// Display a message in a given color 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color">default to white</param>
        public static void Display(string message, ConsoleColor color = ConsoleColor.White)
        {
            var lastcolor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = lastcolor;
        }
        public static void Newline()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// return an stdin user input amongst a selection of chars
        /// </summary>
        /// <param name="choices"></param>
        /// <returns></returns>
        public static char ReadUserChoice(char[] choices)
        {
            while (true)
            {
                var key = Console.ReadKey(true).KeyChar;
                if (choices.Contains(key))
                    return key;
            }
        }
    }
}
