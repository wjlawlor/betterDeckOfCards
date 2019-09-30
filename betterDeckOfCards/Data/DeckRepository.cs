using System;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace betterDeckOfCards.Data
{
    public class DeckRepository : IDeckRepository
    {
        async public Task<Deck> CreateNewShuffledDeckAsync(int deckCount)
        {
            var random = new Random();

            var suits = new[] { "HEARTS", "SPADES", "CLUBS", "DIAMONDS" };
            var values = new[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "JACK", "QUEEN", "KING" };
            var cards = new Card[52 * deckCount];
            var deck = new Deck { DeckId = random.Next().ToString("X") };

            int newCardIndex = 0;
            for (int _ = 0; _ < deckCount; _ += 1)
            {
                foreach (string suit in suits)
                {
                    foreach (string value in values)
                    {
                        string code = value.Substring(0, 1) + suit.Substring(0, 1);
                        if (value == "10")
                        {
                            code = "0" + suit.Substring(0, 1);
                        }
                        cards[newCardIndex] = new Card
                        {
                            Deck = deck,
                            Value = value,
                            Suit = suit,
                            Code = code,
                        };
                        newCardIndex += 1;
                    }
                }
            }

            // Fisher-Yates Shuffle
            for (int cardIndex = cards.Length - 1; cardIndex >= 0; cardIndex -= 1)
            {
                int swapIndex = random.Next(0, cardIndex);
                Card card = cards[swapIndex];
                cards[swapIndex] = cards[cardIndex];
                cards[cardIndex] = card;
                cards[cardIndex].Order = cardIndex;
                cards[swapIndex].Order = swapIndex;
            }

            foreach (Card card in cards)
            {
                deck.Cards.Add(card);
            }

            using (var context = new DeckContext())
            {
                context.Decks.Add(deck);
                await context.SaveChangesAsync();
            }

            return deck;
        }

        async public Task<Deck> GetDeck(string deckId)
        {
            using (var context = new DeckContext())
            {
               Deck deck = await context.Decks
                    .Include(x => x.Cards)
                    .Include(x => x.Piles)
                    .SingleAsync(x => x.DeckId == deckId);

                return deck;
            }
        }

        async public Task<Pile> GetPile(string deckId, string pileName)
        {
            using (var context = new DeckContext())
            {
                Deck deck = await GetDeck(deckId);
                context.Decks.Attach(deck);
                Pile pile = deck.Piles.FirstOrDefault(x => x.Name == pileName);

                if(pile == null)
                {
                    pile = new Pile()
                    {
                        Name = pileName,
                        DeckId = deck.Id,
                        Deck = deck
                    };
                    context.Piles.Add(pile);
                }

                await context.SaveChangesAsync();

                return pile;
            }
        }

        async public Task<Card> GetCard(string deckId, string value)
        {
            using (var context = new DeckContext())
            {
                Deck deck = await GetDeck(deckId);
                Card card = deck.Cards.FirstOrDefault(x => x.Code == value);

                return card;
            }
        }

        async public Task<Pile> PutCardsInPile (string deckId, string pileName, List<string> codes)
        {
            //Deck deck = await GetDeck(deckId);
            //Pile pile = deck.Piles.FirstOrDefault(x => x.Name == pileName);
            
            using (var context = new DeckContext())
            {
                Pile pile = await context.Piles.FirstOrDefaultAsync(x => x.Name == pileName && x.Deck.DeckId == deckId);
                List<Card> ourCards = await context.Cards.Where(x => codes.Contains(x.Code)).ToListAsync();
                foreach (Card card in ourCards)
                {
                    card.Pile = pile;
                    pile.Cards.Add(card);
                }
                await context.SaveChangesAsync();
                return pile;
            }
        }

        async public Task<Deck> DrawCardsAsync(string deckId, int numberToDraw)
        {
            using (var context = new DeckContext())
            {
                Deck deck = await context.Decks
                  .Include(x => x.Cards)
                  .SingleAsync(x => x.DeckId == deckId);

                foreach (Card card in deck.Cards)
                {
                    if (!card.Drawn)
                    {
                        card.Drawn = true;
                        numberToDraw--;
                    }
                    if (numberToDraw == 0)
                    {
                        break;
                    }
                }

                await context.SaveChangesAsync();

                return deck;
            }
        }

        
       
    }
}
