namespace betterDeckOfCards.Models
{
    public class CardDrawRequest
    {
        public CardDrawRequest()
        {
            Count = 1;
        }

        public int? Count { get; set; }
    }
}