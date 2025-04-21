using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Enums;

namespace BlackjackGame.Models
{
    public class Card
    {
        public Suit Suit {get;private set;}
        public Rank Rank {get;private set;}

        public Card (Suit suit, Rank rank){
            Suit = suit;
            Rank = rank;
        }
    }
}