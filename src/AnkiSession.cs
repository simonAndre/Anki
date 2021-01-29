using System.Linq;
using System.Collections.Generic;
using Anki.dataobjects;

namespace Anki
{


    public class AnkiSession
    {
        private List<CardState> _cardSet;


        /// <summary>
        /// Session ctor
        /// </summary>
        /// <param name="newdeck">list of card loaded from disk</param>
        /// <param name="sessiondata">session data taken from the session store, null for the first sessino</param>
        public AnkiSession(List<Card> newdeck, AnkiSessionData sessiondata = null)
        {
            //_deck = new Dictionary<int, Card>();
            _cardSet = new List<CardState>();
            InitSession(newdeck, sessiondata);
        }


        /// <summary>
        /// lets iterate on the cards to review for this session. 
        /// Are either taken from : 
        /// - the fresh new deck (unshelved state) or 
        /// - the red and orange box
        /// - the red box again (until the red box is empty wich means the end of the session)
        /// </summary>
        /// <returns>an enumerable on tuples composed of :
        /// - the card
        /// - the box where the card was taken from (can be null if the card come from the fresh new deck)</returns>
        public IEnumerable<(Card card, Box? box)> CardsToStudyIterator()
        {
            
            //Selection set extraction : First the unshleved cards, then from the red box then form the orange
            //selection set is copied as the card allocation will change during the session

            var cardsToStudy = new List<CardState>();
            cardsToStudy.AddRange(_cardSet.Where(card => card.CurrentStatus == CardState.Status.unshelved));
            cardsToStudy.AddRange(_cardSet.Where(card => card.Box == Box.red));
            cardsToStudy.AddRange(_cardSet.Where(card => card.Box == Box.orange));


            // this cards go to the Hand of the user
            foreach (var card in cardsToStudy)
            {
                card.CurrentStatus = CardState.Status.inhand;
            }

            // then they are proposed to the user one by one
            foreach (var card in cardsToStudy)
            {
                yield return (card, card.Box);
            }

            // while any card are in the red box, we keep on iterating on this box
            foreach (var card in BoxContent(Box.red))
            {
                yield return (card, card.Box);
            }
        }


        /// <summary>
        /// card reordering for the next session
        /// </summary>
        public void ReorderCards()
        {
            foreach (var card in _cardSet)
            {
                if (card.Box.HasValue)
                {
                    switch (card.Box.Value)
                    {
                        case Box.green:
                            card.Box = Box.orange;
                            break;
                        case Box.orange:
                            card.Box = Box.red;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// put a card in  given box
        /// </summary>
        /// <param name="card"></param>
        /// <param name="box"></param>
        public void ShelveCard(Card card, Box shelvingbox)
        {
            var cardinstance = _cardSet.Single(c => c == card);
            cardinstance.Box = shelvingbox;
            cardinstance.CurrentStatus = CardState.Status.shelved;
        }




        /// <summary>
        /// dayof this session
        /// </summary>
        public int CurrentDay { get; private set; }


        public int BoxCardsCount(Box box)
            => BoxContent(box).Count();

        public int InHandCardsCount()
           => _cardSet.Where(c => c.CurrentStatus == CardState.Status.inhand).Count();




        /// <summary>
        /// return true when all the card are in the green box, this is the end of the game.
        /// </summary>
        /// <returns></returns>
        public bool GlobalSuccess()
            => BoxContent(Box.green).Count() == DeckSize;



        /// <summary>
        /// nb of card in the deck (shelved or unshelved)
        /// </summary>
        public int DeckSize
            => _cardSet.Count;



        /// <summary>
        /// get the session data for persistent storage
        /// </summary>
        /// <returns></returns>
        public AnkiSessionData GetSessiondata()
        {
            var sessiondata = new AnkiSessionData()
            {
                Day = CurrentDay,
                CardsSet = _cardSet
            };

            return sessiondata;
        }


        private void InitSession(List<Card> loadedcards, AnkiSessionData sessiondata = null)
        {
            if (sessiondata == null)
            {
                CurrentDay = 1;
                foreach (var card in loadedcards.OrderBy(c => c.Question))
                {
                    CardState cardstate = new CardState(card.Question, card.Answer);
                    _cardSet.Add(cardstate);
                }
            }
            else
            {
                CurrentDay = sessiondata.Day + 1;
                _cardSet = sessiondata.CardsSet;
            }
        }

        /// <summary>
        /// get the content of a box
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        private IEnumerable<CardState> BoxContent(Box box)
            => _cardSet.Where(c => c.CurrentStatus == CardState.Status.shelved && c.Box == box);



    }
}
