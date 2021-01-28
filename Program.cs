using System;

namespace Anki
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                AnkiGame game = new AnkiGame();

                game.Play();
            } while (true);
        }
    }
}
