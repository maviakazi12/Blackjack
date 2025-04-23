using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Interfaces;
using BlackjackGame.Models;

namespace BlackjackGame.Engine
{
    public class Engine
    {
        private IDeck _deck;
        public Engine(IDeck deck)
        {
            _deck = deck;
        }

        public void StartGame()
        {
            _deck.InitializeDeck();
            _deck.Shuffle();
            _deck.Draw(2);
        }

        public void DealCards()
        {

        }
    }
}