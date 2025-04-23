using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Interfaces;
using BlackjackGame.Models;

namespace BlackjackGame.Engine
{
    public class GameController
    {
        private IDeck _deck;
        private IPlayer _player;
        private IPlayer _dealer;

        public int _initialCardsToStartWith;
        public GameController(IDeck deck, IPlayer player, IPlayer dealer, int initialCards)
        {   
            if (deck == null) throw new ArgumentNullException(nameof(deck));
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (dealer == null) throw new ArgumentNullException(nameof(dealer));
            if (initialCards < 2 || initialCards > 4) throw new ArgumentException("Initial number of cards should be between 2 and 4");

        
            _deck = deck;
            _player = player;
            _dealer = dealer;
            _initialCardsToStartWith = initialCards;

        }

        public void Run()
        {
            StartGame();
        }
        private void StartGame()
        {
            _deck.InitializeDeck();
            _deck.Shuffle();
            _deck.Draw(_initialCardsToStartWith);
            _player.ReceiveCards(_deck.drawnCards);
            _deck.Draw(_initialCardsToStartWith);
            _dealer.ReceiveCards(_deck.drawnCards);

        }

    
    }
}