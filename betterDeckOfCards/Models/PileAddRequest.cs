using betterDeckOfCards.Data;
using System.Collections.Generic;

namespace betterDeckOfCards.Models
{
    public class PileAddRequest
    {
        public PileAddRequest()
        {
            Value = new List<string>();
        }

        public List<string> Value { get; set; }
    }
}