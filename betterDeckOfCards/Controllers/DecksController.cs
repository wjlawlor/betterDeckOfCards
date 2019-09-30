using betterDeckOfCards.Data;
using betterDeckOfCards.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace betterDeckOfCards.Controllers
{
    [RoutePrefix("api/decks")]
    public class DecksController : ApiController
    {
        private IDeckRepository _repository;

        public DecksController(IDeckRepository repository)
        {
            _repository = repository;
        }

        async public Task<ShortDeckInfo> Post(DeckCreate creation)
        {
            int creationCount = creation.Count.HasValue ? creation.Count.Value : 1;
            Deck deck = await _repository.CreateNewShuffledDeckAsync(creationCount);
            ShortDeckInfo deckInfo = new ShortDeckInfo
            {
                DeckId = deck.DeckId,
                Remaining = deck.Cards.Where(x => !x.Drawn).Count()
            };
            return deckInfo;
        }

        [Route("{deckId}/cards")]
        async public Task<CardDrawnResponse> Delete(string deckId, CardDrawRequest request)
        {
            int drawCount = request.Count.HasValue ? request.Count.Value : 1;
            Deck deck = await _repository.DrawCardsAsync(deckId, drawCount);
            List<CardInfo> cards = deck.Cards
              .Where(x => x.Drawn)
              .Reverse()
              .Take(drawCount)
              .Reverse()
              .Select(x => new CardInfo { Suit = x.Suit, Value = x.Value, Code = x.Code })
              .ToList();
            return new CardDrawnResponse
            {
                DeckId = deckId,
                Remaining = deck.Cards.Where(x => !x.Drawn).Count(),
                Removed = cards,
            };
        }

        [Route("{deckId}/piles/{pileName}")]
        async public Task<PileAddedResponse> Patch(string deckId, string pileName, PileAddRequest request)
        {
            Deck deck = await _repository.GetDeck(deckId);
            Pile pile = await _repository.GetPile(deckId, pileName);

            List<string> values = request.Value;
            List<Card> cards = new List<Card>();

            foreach(var value in values)
            {
                Card card = await _repository.GetCard(deckId, value);
                cards.Add(card);
            }

            pile = await _repository.PutCardsInPile(deckId, pileName, request.Value);
            deck = await _repository.GetDeck(deckId);

            Dictionary<string, ShortPileInfo> piles = new Dictionary<string, ShortPileInfo>();

            foreach(var individualPile in deck.Piles)
            {
                ShortPileInfo info = new ShortPileInfo();
                info.Remaining = individualPile.Cards.Count;
                piles.Add(pileName, info);
            }

            return new PileAddedResponse
            {
                DeckId = deckId,
                Remaining = deck.Cards.Where(x => !x.Drawn).Count(),
                Piles = piles
            };
        }

        [Route("{deckId}/piles/{pileName}/shuffle")]
        async public Task<HttpResponseMessage> Post(string deckId, string pileName)
        {
            await _repository.Shuffle(deckId, pileName);
            return new HttpResponseMessage(HttpStatusCode.NotModified);
        }
    }
}
