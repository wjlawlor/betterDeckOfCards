﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace betterDeckOfCards.Data
{
    public class Card
    {
        public int Id { get; set; }
        public bool Drawn { get; set; }

        [Column("CardOrder")]
        public int Order { get; set; }

        [Required]
        [MaxLength(10)]
        public string Value { get; set; }

        [Required]
        [MaxLength(8)]
        public string Suit { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        public string Code { get; set; }

        [Required]
        public Deck Deck { get; set; }
        public int DeckId { get; set; }

        public Pile Pile { get; set; }
        public int? PileId { get; set; }
    }
}