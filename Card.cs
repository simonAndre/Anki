using System.Text.Json.Serialization;

namespace Anki
{
    /// <summary>
    /// flashcard to study
    /// the <see cref="CurrentBox"/> property is supposed to be updated by the player given his knowledge on this question
    /// </summary>
    public class Card
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        public Card(string question,string answer)
        {
            this.Question = question;
            this.Answer = answer;
        }
        public Card()
        {
        }
        public int CardId { get;  set; }
        public string Question { get;  set; }
        public string Answer { get;  set; }

        /// <summary>
        /// flag to signal a done card for a round
        /// </summary>
        [JsonIgnore]
        public bool Rounddone { get; set; }


        ///// <summary>
        ///// box in wich the card resides currently
        ///// if null => the card hasn't still been played (new deck)
        ///// </summary>
        //public Box? CurrentBox { get; set; }

    }
}
