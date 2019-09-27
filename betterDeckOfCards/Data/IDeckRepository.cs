using System.Threading.Tasks;

namespace betterDeckOfCards.Data
{
    public interface IDeckRepository
    {
        Task<Deck> CreateNewShuffledDeckAsync(int deckCount);
    }
}