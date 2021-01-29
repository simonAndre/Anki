using Anki.dataobjects;
using Anki.utils;
using System;

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
                Helpers.Display(new String('-', 30));
                Helpers.Display("Hi, welcome to the ANKI learning game");
                Helpers.Display(new String('-', 30));
                Helpers.Newline(2);
                if (_session.CurrentDay == 1)
                {
                    Helpers.Display("this your first session");
                }
                else
                {
                    DisplayBoxes("Last session review", true);
                    Helpers.Display($"Be prepared for the session of the day #{_session.CurrentDay}");

                }
                Helpers.Newline(2);


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

                bool result = EndSessionQuizz();

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
            foreach (var (card, frombox) in _session.CardsToStudyIterator())
            {
                DisplayBoxes("Flashcards studying session");
                if (frombox.HasValue)
                {
                    var lastcolor=Console.ForegroundColor;
                    Console.Write("Card taken from the ");
                    Console.ForegroundColor =Helpers.BoxColor(frombox.Value);
                    Console.Write($"{frombox} box");
                    Console.ForegroundColor = lastcolor;
                    Console.WriteLine(", the question is :");
                }
                else
                {
                    Helpers.Display($"New card to study, the question is :");
                }
                Helpers.Display(card.Question, ConsoleColor.Cyan);
                Helpers.Newline();
                Helpers.Pause("for the answer");

                Helpers.Display($"Answer: {card.Answer}", ConsoleColor.DarkBlue);
                Helpers.Newline();
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
            DisplayBoxes("End of the session");
            bool endOfGame = false;

            if (_session.GlobalSuccess())
            {
                Helpers.Display($"Congratulations, you have learned all the {_session.DeckSize} cards in {_session.CurrentDay} days", ConsoleColor.Green);
                endOfGame= true;
            }
            else
            {
                Helpers.Display("We are going to reorder the cards for the next session");
                Helpers.Pause("to reorder.");
                _session.ReorderCards();
                DisplayBoxes("Reordering stage");
                Helpers.Display($"End of the study session for today, sleep well !");
                Helpers.Display($"To play session for the next day, please rerun the Anki program");
            }

            Helpers.Pause("to exit.");
            return endOfGame;
        }

        /// <summary>
        /// display the cards contained in each box
        /// </summary>
        private void DisplayBoxes(string subtitle, bool recapLastSession = false)
        {
            Helpers.ClearDisplay();
            Helpers.Display(new String('-', 40));
            Helpers.Display($"ANKI learning game - Day {_session.CurrentDay}");
            Helpers.Display(subtitle);
            Helpers.Display(new String('-', 40));
            Helpers.Newline();

            Helpers.Display("Content of boxes:");

            Helpers.Display($"1- GREEN BOX (to study in 2 days) : {_session.BoxCardsCount(Box.green)} cards", ConsoleColor.Green);
            Helpers.Display($"2- ORANGE BOX (to study tomorrow) : {_session.BoxCardsCount(Box.orange)} cards", ConsoleColor.DarkYellow);
            Helpers.Display($"3- RED BOX (to study today) :       {_session.BoxCardsCount(Box.red)} cards", ConsoleColor.Red);
            if (!recapLastSession)
            {
                Helpers.Display(new String('-', 40));
                Helpers.Display($"In your hand to study :         {_session.InHandCardsCount()} cards", ConsoleColor.Yellow);
            }
            Helpers.Display(new String('_', 40));
            Helpers.Newline(2);
        }



    }
}
