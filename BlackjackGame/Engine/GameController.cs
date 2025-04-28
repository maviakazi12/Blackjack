using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Enums;
using BlackjackGame.Interfaces;
using BlackjackGame.Models;

namespace BlackjackGame.Engine
{
    public class GameController
    {
        private IDeck _deck;
        private IPlayer _player;
        private IPlayer _dealer;
        private IIO _inputOutput;
        private IScoringService _scoring;
        private GameState _gameState;
        public GameState GameState => _gameState; // to expose _gameState in test

        public int _initialCardsToStartWith;
        public GameController(IDeck deck, IPlayer player, IPlayer dealer, int initialCards, IIO inputOutput, IScoringService scoring)
        {
            if (deck == null) throw new ArgumentNullException(nameof(deck));
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (dealer == null) throw new ArgumentNullException(nameof(dealer));
            if (initialCards < 2 || initialCards > 4) throw new ArgumentException("Initial number of cards should be between 2 and 4");

            _gameState = new GameState();
            _deck = deck;
            _player = player;
            _dealer = dealer;
            _initialCardsToStartWith = initialCards;
            _inputOutput = inputOutput;
            _scoring = scoring;

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

        public void PlayPlayerTurn()
        {
            string playerChoice = _inputOutput.GetPlayerChoice();
            while (playerChoice == "hit")
            {
                _deck.Draw(1);
                _player.ReceiveCards(_deck.drawnCards);
                _gameState.CurrentTurn = PlayerType.Dealer;
                playerChoice = _inputOutput.GetPlayerChoice();
            }
            _gameState.CurrentTurn = PlayerType.Dealer;
        }

        public void PlayDealerTurn()
        {
            int dealerScore = _scoring.CalculateScore(_dealer.CardsInHand);

            while (dealerScore < 17){
                _deck.Draw(1);
                _dealer.ReceiveCards(_deck.drawnCards);
                dealerScore = _scoring.CalculateScore(_dealer.CardsInHand);
            }

        }

        public void ResetGame() { }

    }

}