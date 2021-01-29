using System.Text.Json.Serialization;

namespace Anki.dataobjects
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
        public string Question { get;  set; }
        public string Answer { get;  set; }

        /// <summary>
        /// card "key" is made by its "question" field
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.Question == (obj as Card)?.Question;
        }

        public override int GetHashCode()
        {
            return Question.GetHashCode();
        }
    }

}
