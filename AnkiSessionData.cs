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
            CardsShelving = new List<(int, int)>();
        }
        public int Day { get; set; } = 0;
        public List<(int card, int box)> CardsShelving { get; set; }
    }
}
