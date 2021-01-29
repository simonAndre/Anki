using Anki.dataobjects;
using System;
using System.Linq;

namespace Anki.utils
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
            Display($"please, press any key {nextActionInfo}", ConsoleColor.Gray);
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

        public static ConsoleColor BoxColor(Box box)
        {
            switch (box)
            {
                case Box.green:
                    return ConsoleColor.Green;
                case Box.orange:
                    return ConsoleColor.DarkYellow;
                case Box.red:
                    return ConsoleColor.Red;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void Newline(int number=1)
        {
            for (int i = 0; i < number; i++)
            {
                Console.WriteLine();
            }
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
