using betterDeckOfCards.Data;
using System.Collections.Generic;

namespace betterDeckOfCards.Models
{
    public class PileAddedResponse
    {
        public PileAddedResponse()
        {

        }

        public string DeckId { get; set; }
        public int Remaining { get; set; }

        public Dictionary<string, ShortPileInfo> Piles { get; set; }
    }
}