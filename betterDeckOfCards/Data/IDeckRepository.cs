using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace betterDeckOfCards.Data
{
    public interface IDeckRepository
    {
        Task<Deck> CreateNewShuffledDeckAsync(int deckCount);
        Task<Deck> GetDeck(string deckId);
        Task<Pile> GetPile(string deckId, string pileName);
        Task<Card> GetCard(string deckId, string value);
        Task<Pile> PutCardsInPile(string deckId, string pileName, List<string> codes);
        Task<Deck> DrawCardsAsync(string deckId, int numberToDraw);
        Task<HttpResponseMessage> Shuffle(string deckId, string pileName);
    }
}