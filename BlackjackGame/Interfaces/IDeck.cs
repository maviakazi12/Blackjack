using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Models;

namespace BlackjackGame.Interfaces
{
    public interface IDeck
    {
        public List<Card> deckOfCards { get; }
        public List<Card> drawnCard { get; }
        void InitializeDeck();
        void Shuffle();
    }
}