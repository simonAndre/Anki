using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Anki
{
    public class AnkiGame
    {
        private AnkiSession _session;
        private Persistance<AnkiSessionData> _persistance;
        public AnkiGame()
        {
            _persistance = new Persistance<AnkiSessionData>("deck.txt");
        }

        /// <summary>
        /// run a playing session for a day
        /// </summary>
        /// <returns>true if the user successfully end the session (all cards are green)</returns>
        public bool Play()
        {
            try
            {

                //first, loads the deck from disc
                var deck = _persistance.LoadDeck();
                //load the session data from disc (if any)
                var sessiondata = _persistance.LoadSessionState();

                _session = new AnkiSession(deck, sessiondata);


                Helpers.ClearDisplay();
                Helpers.Display($"Hi, welcome to your Anki session for the day #{_session.CurrentDay}");
                Helpers.Newline();
                Helpers.Newline();

                if (sessiondata == null)
                {
                    Helpers.Pause("to start");
                }
                else
                {
                    Helpers.Display("press s to start the day session");
                    Helpers.Display("press x to delete the last saved session and start again from a brand new deck");
                    char userchoice = Helpers.ReadUserChoice(new char[] { 's', 'x' });
                    if (userchoice == 'x')
                    {
                        _persistance.DropSessionStore();
                        return Play();
                    }
                }

                StartSessionQuizz();

                bool result=EndSessionQuizz();

                if (result)
                {
                    _persistance.DropSessionStore();
                }
                else
                {
                    _persistance.PersistSessionState(_session.GetSessiondata());
                }

                return result;
            }
            catch (Exception ex)
            {
                Helpers.Display($"We are sorry but an error occured : {ex.Message}.", ConsoleColor.Red);
                Helpers.Display("Exiting program.");
                return false;
            }
        }


        /// <summary>
        /// start the quizz for the new cards or the cards in the red box
        /// </summary>
        public void StartSessionQuizz()
        {
            DisplayBoxes();
            Helpers.Pause($"Start the session of day {_session.CurrentDay}");

            foreach (var card in _session.GetCardsToStudy())
            {
                DisplayBoxes();
                Helpers.Display($"Card study, the question is :");
                Helpers.Display(card.Question, ConsoleColor.Cyan);
                Helpers.Pause("for the answer");

                Helpers.Display($"Answer: {card.Answer}", ConsoleColor.DarkBlue);
                Helpers.Display("Please rank this card in a box according to your knowledge for this question : geen box=1, orange box=2, red box=3)");
                var newbox = Helpers.ReadBoxKey();
                _session.ShelveCard(card, newbox);
            }
        }

        /// <summary>
        /// end of the session (there are no more cards in the red box),
        /// check is the global success is achieve or reorder the cards for the next session
        /// </summary>
        public bool EndSessionQuizz()
        {
            DisplayBoxes();

            if (_session.GlobalSuccess())
            {
                Helpers.Display($"Congratulations, you have learned all the {_session.DeckSize} cards in {_session.CurrentDay} days", ConsoleColor.Green);
                return true;
            }
            else
            {
                Helpers.Display("We are going to reorder the cards for the next session");
                Helpers.Pause("to reorder.");
                _session.ReorderCards();
                DisplayBoxes();
                Helpers.Display($"End of the study session for today, sleep well!");
            }

            Helpers.Pause("to exit.");
            return false;
        }

        /// <summary>
        /// display the cards contained in each box
        /// </summary>
        private void DisplayBoxes()
        {
            Helpers.ClearDisplay();
            Helpers.Display("The Anki boxes:");
            Helpers.Display($"1- GREEN BOX (to study in 2 days) : {_session.Boxes[Box.green].Count} cards", ConsoleColor.Green);
            Helpers.Display($"2- ORANGE BOX (to study tomorrow) : {_session.Boxes[Box.orange].Count} cards", ConsoleColor.Yellow);
            Helpers.Display($"3- RED BOX (to study today) :       {_session.Boxes[Box.red].Count} cards", ConsoleColor.Red);
            Helpers.Display(new String('_', 20));
            Helpers.Newline();
        }



    }
}
