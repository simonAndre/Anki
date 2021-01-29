using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using Anki.dataobjects;

namespace Anki.utils
{
    public class Persistance<T>
    {
        private const string _persistedSession_filename = "session.json";
        private string _deck_filename = string.Empty;
        private string _workingDir= string.Empty;

        public Persistance(string deck_filename = "deck.txt", string workingdirectory=null)
        {
            _deck_filename = deck_filename;
            _workingDir = workingdirectory ?? Environment.CurrentDirectory;
        }

        private string SessionFile => Path.Combine(_workingDir, _persistedSession_filename);

        private string DeckFile => Path.Combine(_workingDir, _deck_filename);

        /// <summary>
        /// load the session state from the last session
        /// create the first Session if no statefile was found wich means that we start with a new deck.
        /// </summary>
        /// <returns>the session or null if no session has been stored</returns>
        public T LoadSessionState()
        {
            try
            {
                if (File.Exists(SessionFile))
                {
                    try
                    {
                        var jsondata = File.ReadAllText(SessionFile);
                        return JsonSerializer.Deserialize<T>(jsondata);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"error parsing the session from the file {SessionFile} : {ex.Message}");
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting data from the filesystem : {ex.Message}");
            }
        }
        public void DropSessionStore()
        {
            if (File.Exists(SessionFile))
            { 
                File.Delete(SessionFile); 
            }
        }
        public void PersistSessionState(T sessiondata)
        {
            try
            {
                var sessionjsondata = JsonSerializer.Serialize(sessiondata);
                File.WriteAllText(SessionFile, sessionjsondata);
            }
            catch (Exception ex)
            {
                throw new Exception($"error persising the session to the file {SessionFile} : {ex.Message}");
            }
        }


        /// <summary>
        /// load a new deck from the text file (separator is '|' )
        /// </summary>
        /// <returns></returns>
        public List<Card> LoadDeck()
        {
            try
            {
                //load of the new deck for the first session
                var deckdatalines = File.ReadAllLines(DeckFile);

                //basic parsing 
                var newdeck = new List<Card>();
                foreach (var deckline in deckdatalines)
                {
                    if (deckline.Trim().ToLower() == "card question|card answer")
                        //we skip the header line
                        continue;

                    var decklineParts = deckline.Split('|');
                    if (decklineParts.Length != 2)
                        throw new Exception("bad format for card, each line must be formated as : [question|answer]");
                    var newcard = new Card(decklineParts[0], decklineParts[1]);
                    newdeck.Add(newcard);
                }
                return newdeck;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while loading the deck file : {ex.Message}", ex);
            }
        }

    }
}
