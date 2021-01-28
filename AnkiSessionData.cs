using System.Collections.Generic;
namespace Anki
{
    /// <summary>
    /// 
    /// </summary>
    public class AnkiSessionData
    {
        public AnkiSessionData()
        {
            CardsShelving = new Dictionary<string, Box>();
        }
        public int Day { get; set; } = 1;

        /// <summary>
        /// card shelving are stored using the card question as a key
        /// </summary>
        public Dictionary<string , Box> CardsShelving { get; set; }
    }
}
