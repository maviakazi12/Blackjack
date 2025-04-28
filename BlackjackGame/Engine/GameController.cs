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
        private int playerScore;
        private int dealerScore;


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
            dealInitialHands();   // Deals cards to player and dealer

            PlayPlayerTurn();    // Player hits or stays until done

            PlayDealerTurn();    // Dealer plays according to their rules

            HandleWinOrLoss();   // Announce who won

            ResetGame();         // Reset for next game 
        }
        private void dealInitialHands()
        {
            _deck.InitializeDeck();
            _deck.Shuffle();
            _deck.Draw(_initialCardsToStartWith);
            _player.ReceiveCards(_deck.drawnCards);
            _inputOutput.DisplayPlayerHand(_player.CardsInHand);
            playerScore = _scoring.CalculateScore(_player.CardsInHand);
            _inputOutput.DisplayScore(PlayerType.Player.ToString(), playerScore);

            _deck.Draw(_initialCardsToStartWith);
            _dealer.ReceiveCards(_deck.drawnCards);
            _inputOutput.DisplayDealerHand(_dealer.CardsInHand);
            dealerScore = _scoring.CalculateScore(_dealer.CardsInHand);
            _inputOutput.DisplayScore(PlayerType.Dealer.ToString(), dealerScore);

        }

        public void PlayPlayerTurn()
        {
            string playerChoice = _inputOutput.GetPlayerChoice();
            _gameState.IsPlayerBust = false;
            _gameState.IsPlayerWinner = false;
            while (playerChoice == "hit" && !_gameState.IsPlayerBust && !_gameState.IsPlayerWinner)
            {
                _deck.Draw(1);
                _player.ReceiveCards(_deck.drawnCards);
                playerScore = _scoring.CalculateScore(_player.CardsInHand);
                _inputOutput.DisplayPlayerHand(_player.CardsInHand);
                _inputOutput.DisplayScore(PlayerType.Player.ToString(), playerScore);
                _gameState.IsPlayerBust = IsBust(playerScore);
                _gameState.IsPlayerWinner = IsBlackjack(playerScore);

                playerChoice = _inputOutput.GetPlayerChoice();

            }

            _gameState.CurrentTurn = PlayerType.Dealer;
        }

        public void PlayDealerTurn()
        {
            string dealerChoice;
            int dealerScore = _scoring.CalculateScore(_dealer.CardsInHand);
            _gameState.IsDealerBust = false;
            _gameState.IsDealerWinner = false;

            while (dealerScore <= 17 && !_gameState.IsDealerBust && !_gameState.IsDealerWinner)
            {
                dealerChoice = "hit";
                _inputOutput.DisplayDealerChoice(dealerChoice);
                _deck.Draw(1);
                _dealer.ReceiveCards(_deck.drawnCards);
                dealerScore = _scoring.CalculateScore(_dealer.CardsInHand);
                _inputOutput.DisplayDealerHand(_dealer.CardsInHand);
                _inputOutput.DisplayScore(PlayerType.Dealer.ToString(), dealerScore);
                _gameState.IsDealerBust = IsBust(dealerScore);
                _gameState.IsDealerWinner = IsBlackjack(dealerScore);

                if (dealerScore > 17 && !_gameState.IsDealerBust && !_gameState.IsDealerWinner)
                {
                    dealerChoice = "stay";
                    _inputOutput.DisplayDealerChoice(dealerChoice);
                }

            }
        }

        public bool IsBust(int score) => score > 21;

        public bool IsBlackjack(int score) => score == 21;


        public void HandleWinOrLoss()
        {
            if (_gameState.IsDealerBust || _gameState.IsPlayerWinner)
            {
                _inputOutput.AnnounceWinner("Player", "Dealer");
            }
            else if (_gameState.IsPlayerBust || _gameState.IsDealerWinner)
            {
                _inputOutput.AnnounceWinner("Dealer", "Player");
            }
            else
            {
                CompareFinalScores();
            }
        }
        private void CompareFinalScores()
        {
            int playerFinalScore = _scoring.CalculateScore(_player.CardsInHand);
            int dealerFinalScore = _scoring.CalculateScore(_dealer.CardsInHand);

            if (playerFinalScore < dealerFinalScore)
            {
                _inputOutput.AnnounceWinner("Dealer", "Player");
            }
            else if (playerFinalScore > dealerFinalScore)
            {
                _inputOutput.AnnounceWinner("Player", "Dealer");
            }
            else
            {
                _inputOutput.DrawGame();
            }
        }
        public void ResetGame()
        {
            _player.CardsInHand.Clear();
            _dealer.CardsInHand.Clear();
            playerScore = 0;
            dealerScore = 0;
            _deck.deckOfCards.Clear();
            _gameState = new GameState();

        }

    }

}