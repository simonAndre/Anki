using System.Collections.Generic;
namespace Anki.dataobjects
{
    /// <summary>
    /// 
    /// </summary>
    public class AnkiSessionData
    {
        public AnkiSessionData()
        {
            CardsSet = new List<CardState>();
        }
        public int Day { get; set; } 

        public List<CardState> CardsSet { get; set; }
    }
}
