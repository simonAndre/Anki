using System;

namespace Anki
{
    class Program
    {
        private const string _default_datastore_directoryName = "datastore";
        static void Main(string[] args)
        {
            // retrieving working directory for getting session state file and deck.txt file (liste of quesitons)
            // first from program args
            // second : from the environement variable ANKI_DATASTORE
            // third : from the [current_directory/datastore] dir

            string datastoredir;
            string datastoredir_env = Environment.GetEnvironmentVariable("ANKI_DATASTORE");

            if (args.Length > 0)
            {
                datastoredir = args[0];
            }
            else if (!string.IsNullOrEmpty(datastoredir_env))
            {
                datastoredir = datastoredir_env;
            }
            else
            {
                datastoredir = System.IO.Path.Combine( Environment.CurrentDirectory,"datastore");
            }


            //launch the game.

            AnkiGame game = new AnkiGame(datastoredir);
            game.Play();
        }
    }
}
