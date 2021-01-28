using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace Anki
{
    public struct ShelvedCard
    {
        public Card Card;
        public Box Box;
    }
    public class AnkiSession
    {
        private List<Card> _unshelveCards = new List<Card>();


        /// <summary>
        /// Session ctor
        /// </summary>
        /// <param name="newdeck">list of card loaded from disk</param>
        /// <param name="sessiondata">when a last session exists</param>
        public AnkiSession(List<Card> newdeck, AnkiSessionData sessiondata = null)
        {
            //_deck = new Dictionary<int, Card>();
            Boxes = new Dictionary<Box, Queue<Card>>();

            InitSession(newdeck, sessiondata);
        }

        public int CurrentDay { get; private set; }
        public Dictionary<Box, Queue<Card>> Boxes { get; private set; }

        public AnkiSessionData GetSessiondata()
        {
            var sessiondata = new AnkiSessionData()
            {
                Day = CurrentDay
            };
            foreach (var box in Boxes.Keys)
            {
                foreach (var card in Boxes[box])
                {
                    sessiondata.CardsShelving.Add(card.Question, box);
                }
            }
            var sessionjsondata = JsonSerializer.Serialize(sessiondata);

            return sessiondata;
        }


        /// <summary>
        /// card reordering for the next session
        /// </summary>
        public void ReorderCards()
        {
            Card card;
            do
            {
                card = RetrieveCardFromBox(Box.orange);
                if (card != null)
                    Boxes[Box.red].Enqueue(card);
            } while (card != null);

            do
            {
                card = RetrieveCardFromBox(Box.green);
                if (card != null)
                    Boxes[Box.orange].Enqueue(card);
            } while (card != null);
        }



        /// <summary>
        /// return true when all the card are in the green box, this is th end of the game.
        /// </summary>
        /// <returns></returns>
        public bool GlobalSuccess()
            => Boxes[Box.green].Count == DeckSize;



        /// <summary>
        /// nb of card in the deck (ie: sum of the cards in each boxes included unshelves)
        /// </summary>
        public int DeckSize
            => Boxes.Sum(box => box.Value.Count());





        /// <summary>
        /// lets iterate on the cards to review for this session round. 
        /// Are either taken from : 
        /// - the new deck (unshelved state) or 
        /// - the red and orange box for the first round 
        /// - the red box only for the next round (until the red box is empty wich means the end of the session)
        /// </summary>
        public IEnumerable<Card> GetCardsToStudy()
        {

            foreach (var unshelvedcard in _unshelveCards)
            {
                yield return unshelvedcard;
            }

            Card card;
            do
            {
                card = RetrieveCardFromBox(Box.red);
                if (card != null)
                    yield return card;
            } while (card != null);
        }



        /// <summary>
        /// put a card in  given box
        /// </summary>
        /// <param name="card"></param>
        /// <param name="box"></param>
        public void ShelveCard(Card card, Box shelvingbox)
        {
            Boxes[shelvingbox].Enqueue(card);
            card.Rounddone = true;
        }


        private Card RetrieveCardFromBox(Box box)
        {
            if (Boxes[box].Any())
                return Boxes[box].Dequeue();
            return null;
        }

        private void InitSession(List<Card> loadedcards, AnkiSessionData sessiondata = null)
        {
            if (sessiondata != null)
                CurrentDay = sessiondata.Day + 1;

            //init the boxes
            foreach (var box in Enum.GetValues(typeof(Box)))
            {
                Boxes.Add((Box)box, new Queue<Card>());
            }

            foreach (var card in loadedcards.OrderBy(c => c.Question))
            {
                if (sessiondata == null)
                {
                    _unshelveCards.Add(card);
                }
                else
                {
                    var box = sessiondata.CardsShelving[card.Question];
                    Boxes[box].Enqueue(card);
                }
            }
        }


    }
}
