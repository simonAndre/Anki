namespace Anki
{
    public enum Box
    {
        /// <summary>
        /// in no box
        /// </summary>
        unshelved=0, 

        /// <summary>
        /// card known, to restudy in 2 days unless all cards are in this box
        /// </summary>
        green = 1,

        /// <summary>
        /// to study tomorow
        /// </summary>
        orange = 2,

        /// <summary>
        /// to study today
        /// </summary>
        red = 3
    }
}
