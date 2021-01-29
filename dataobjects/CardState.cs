namespace Anki.dataobjects
{
    /// <summary>
    /// handle the state of a card
    /// </summary>
    public class CardState : Card
    {
        /// <summary>
        /// Status of a card
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// the card is still in the fresh new deck and hasn't been studied
            /// </summary>
            unshelved,

            /// <summary>
            /// the card is shelved in a box
            /// </summary>
            shelved,

            /// <summary>
            /// the card has been taken from a box for a study
            /// the box field still hold the info of the origin box
            /// </summary>
            inhand
        }

        public CardState()
        {

        }
        public CardState(string question, string answer) :
            base(question, answer)
        {
            CurrentStatus = Status.unshelved;
        }


        /// <summary>
        /// if <see cref="CurrentStatus"/> is <see cref="Status.shelved"/> : the box where the card is
        /// if <see cref="CurrentStatus"/> is <see cref="Status.inhand"/> : the box where the card has been taken from
        /// empty if <see cref="CurrentStatus"/> is <see cref="Status.unshelved"/>
        /// </summary>
        public Box? Box { get; set; }

        /// <summary>
        /// current status of the card
        /// </summary>
        public Status CurrentStatus { get; set; }
    }
}
