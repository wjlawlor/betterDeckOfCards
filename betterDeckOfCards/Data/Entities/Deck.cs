﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace betterDeckOfCards.Data
{
    [Table("Deck")]
    public class Deck
    {
        public Deck()
        {
            Piles = new List<Pile>();
            Cards = new List<Card>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string DeckId { get; set; }

        public IList<Pile> Piles { get; set; }

        public IList<Card> Cards { get; set; }
    }
}