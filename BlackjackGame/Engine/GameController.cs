using BlackjackGame.Enums;
using BlackjackGame.Interfaces;
using BlackjackGame.Models;

namespace BlackjackGame.Engine
{
    public class GameController
    {
        private const int BlackjackScore = 21;
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
            ResetGame();
            _inputOutput.Welcome();
            _gameState.Winner = PlayerType.None;

            _deck.InitializeDeck();
            _deck.Shuffle();
            DealInitialHands(_player, PlayerType.Player);   // Deals cards to player and dealer
            DealInitialHands(_dealer, PlayerType.Dealer);   // Deals cards to player and dealer

            HandleWinOrLoss();

            if (!IsGameOver())
            {

                PlayPlayerTurn();    // Player hits or stays until done

                if (_gameState.IsPlayerBust || _gameState.IsPlayerWinner)
                {
                    HandleWinOrLoss();
                    return;
                }

                PlayDealerTurn();    // Dealer plays according to their rules

                HandleWinOrLoss();
            }

            if (_gameState.Winner == PlayerType.None)
            {
                CompareFinalScores();
            }

            // Reset for next game 
        }
        public void DealInitialHands(IPlayer player, PlayerType type)
        {
            _deck.Draw(_initialCardsToStartWith);
            player.ReceiveCards(_deck.drawnCards);
            _inputOutput.DisplayPlayerHand(player.CardsInHand);
            _inputOutput.DisplayScore(type.ToString(), _scoring.CalculateScore(player.CardsInHand));

        }

        private void ProcessCardDraw(IPlayer player, PlayerType type, out int score, out bool isBust, out bool isBlackjack)
        {
            _deck.Draw(1);
            player.ReceiveCards(_deck.drawnCards);
            score = _scoring.CalculateScore(player.CardsInHand);
            _inputOutput.DisplayPlayerHand(player.CardsInHand);
            _inputOutput.DisplayScore(type.ToString(), score);
            isBust = IsBust(score);
            isBlackjack = IsBlackjack(score);
        }
        
        public void PlayPlayerTurn()
        {
            string playerChoice = _inputOutput.GetPlayerChoice();
            _gameState.IsPlayerBust = false;
            _gameState.IsPlayerWinner = false;
            int score;
            bool isBust;
            bool isBlackjack;

            while (playerChoice == "hit" && !_gameState.IsPlayerBust && !_gameState.IsPlayerWinner)
            {
                ProcessCardDraw(_player, PlayerType.Player, out score, out isBust, out isBlackjack);
                _gameState.IsPlayerBust = isBust;
                _gameState.IsPlayerWinner = isBlackjack;

                if (!_gameState.IsPlayerBust && !_gameState.IsPlayerWinner)
                {
                    playerChoice = _inputOutput.GetPlayerChoice();
                }

            }
        }

        public void PlayDealerTurn()
        {
            int score = GetDealerScore();
            _gameState.IsDealerBust = false;
            _gameState.IsDealerWinner = false;
            bool isBust;
            bool isBlackjack;

            while (score <= 17 && !_gameState.IsDealerBust && !_gameState.IsDealerWinner)
            {
                _inputOutput.DisplayDealerChoice("hit");
                ProcessCardDraw(_dealer, PlayerType.Dealer, out score, out isBust, out isBlackjack);
                _gameState.IsDealerBust = isBust;
                _gameState.IsDealerWinner = isBlackjack;
                if (isBust || isBlackjack)
                    break;
            }
        }

        public bool IsBust(int score) => score > BlackjackScore;

        public bool IsBlackjack(int score) => score == BlackjackScore;


        private bool CheckAndHandleOutcome(bool hasWinningCondition, string winnerName, PlayerType winnerType, string loserName)
        {
            if (hasWinningCondition)
            {
                _inputOutput.AnnounceWinner(winnerName, loserName);
                _gameState.Winner = winnerType;
                return true;
            }
            return false;
        }

        public void HandleWinOrLoss()
        {
            playerScore = GetPlayerScore();
            dealerScore = GetDealerScore();

            _gameState.IsDealerBust = IsBust(dealerScore);
            _gameState.IsDealerWinner = IsBlackjack(dealerScore);
            _gameState.IsPlayerBust = IsBust(playerScore);
            _gameState.IsPlayerWinner = IsBlackjack(playerScore);

            if (CheckAndHandleOutcome(_gameState.IsPlayerBust, "Dealer", PlayerType.Dealer, "Player")) return;
            if (CheckAndHandleOutcome(_gameState.IsDealerBust, "Player", PlayerType.Player, "Dealer")) return;
            if (CheckAndHandleOutcome(_gameState.IsPlayerWinner, "Player", PlayerType.Player, "Dealer")) return;
            if (CheckAndHandleOutcome(_gameState.IsDealerWinner, "Dealer", PlayerType.Dealer, "Player")) return;
        }


        private void CompareFinalScores()
        {
            int playerFinalScore = GetPlayerScore();
            int dealerFinalScore = GetDealerScore();

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

        public bool IsGameOver()
        {
            if (!_gameState.IsDealerBust && !_gameState.IsDealerWinner && !_gameState.IsPlayerBust && !_gameState.IsPlayerWinner) return false;
            return true;
        }

        public void ResetGame()
        {
            _player.Reset();
            _dealer.Reset();
            playerScore = 0;
            dealerScore = 0;
            _deck.deckOfCards.Clear();
            _gameState = new GameState();

        }
        private int GetPlayerScore() => _scoring.CalculateScore(_player.CardsInHand);
        private int GetDealerScore() => _scoring.CalculateScore(_dealer.CardsInHand);


    }

}