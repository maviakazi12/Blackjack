using BlackjackGame.Engine;
using BlackjackGame.Interfaces;
using BlackjackGame.Enums;
using BlackjackGame.Models;
using Moq;
using Moq.AutoMock;
using FluentAssertions;
using BlackjackGame.Services;

namespace BlackjackTests.EngineTests
{
    public class GameControllerTests
    {
        private (GameController controller, AutoMocker mocker) CreateGameController(int initialCards = 2)
        {
            var mocker = new AutoMocker();

            var controller = new GameController(
                mocker.Get<IDeck>(),
                mocker.Get<IPlayer>(),
                mocker.Get<IPlayer>(),
                initialCards,
                mocker.Get<IIO>(),
                mocker.Get<IScoringService>()
            );

            return (controller, mocker);
        }

        [Fact]
        public void dealInitialHands_Should_Call_InitializeDeck_Method()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();
            var player = mocker.Get<IPlayer>();

            // Act
            controller.dealInitialHands(player, PlayerType.Player);

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.InitializeDeck(), Times.Once);
        }

        [Fact]
        public void dealInitialHands_Should_Call_Shuffle_Method()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();
            var player = mocker.Get<IPlayer>();

            // Act
            controller.dealInitialHands(player, PlayerType.Player);

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.Shuffle(), Times.Once);
        }

        [Fact]
        public void dealInitialHands_Should_Deal_n_Number_Of_Cards()
        {
            // Arrange
            var (controller, mocker) = CreateGameController(4);

            // Mock ResetGame to prevent it from interfering with the test
            mocker.GetMock<IPlayer>().Setup(player => player.CardsInHand).Returns(new List<Card>());
            mocker.GetMock<IPlayer>().Setup(dealer => dealer.CardsInHand).Returns(new List<Card>());
            mocker.GetMock<IDeck>().Setup(deck => deck.deckOfCards).Returns(new List<Card>());
            var player = mocker.Get<IPlayer>();

            // Act
            controller.dealInitialHands(player, PlayerType.Player);

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.Draw(4), Times.Once);
        }

        [Fact]
        public void Constructor_Should_Throw_Exception_If_Deck_Is_Null()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new GameController(
                    null,
                    Mock.Of<IPlayer>(),
                    Mock.Of<IPlayer>(),
                    2,
                    Mock.Of<IIO>(),
                    Mock.Of<IScoringService>())
            );
        }


        [Fact]
        public void PlayPlayerTurn_Should_Call_ReceiveCards_On_Player_They_Choose_Hit()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            var drawnCard = new List<Card> { new Card(Suit.heart, Rank.ace) };

            mocker.GetMock<IDeck>().Setup(deck => deck.drawnCards).Returns(drawnCard);
            mocker.GetMock<IIO>().SetupSequence(io => io.GetPlayerChoice()).Returns("hit").Returns("stay");

            // Act
            controller.PlayPlayerTurn();

            // Assert
            mocker.GetMock<IPlayer>().Verify(player => player.ReceiveCards(drawnCard), Times.Once);
        }

        [Fact]
        public void PlayPlayerTurn_Should_Not_Draw_Card_When_Player_Chooses_Stay()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            mocker.GetMock<IIO>().Setup(io => io.GetPlayerChoice()).Returns("stay");

            // Act
            controller.PlayPlayerTurn();

            // Assert
            mocker.GetMock<IPlayer>().Verify(player => player.ReceiveCards(It.IsAny<List<Card>>()), Times.Never());
        }

        [Fact]
        public void PlayPlayerTurn_Should_Set_GameState_To_Dealer_After_Hit_When_Not_Bust_Or_Win()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            mocker.GetMock<IIO>()
            .SetupSequence(io => io.GetPlayerChoice())
            .Returns("hit")   // First call: Hit
            .Returns("stay"); // Second call: Stay

            // Act
            controller.PlayPlayerTurn();

            // Assert
            controller.GameState.CurrentTurn.Should().Be(PlayerType.Dealer);
        }

        [Fact]
        public void Player_Should_Bust_When_Score_Exceeds_21()
        {
            //Arrange
            var(controller,mocker) = CreateGameController();
            mocker.GetMock<IScoringService>()
            .SetupSequence(scoringService => scoringService.CalculateScore(It.IsAny<List<Card>>()))
            .Returns(23);

            mocker.GetMock<IIO>()
            .SetupSequence(io => io.GetPlayerChoice())
            .Returns("hit");

            //Act
            controller.PlayPlayerTurn();

            //Assert
            controller.GameState.IsPlayerBust.Should().BeTrue();
        }

        [Fact]
        public void Player_Should_Win_When_Score_Is_21()
        {
          //Arrange
            var(controller, mocker) = CreateGameController();
            mocker.GetMock<IScoringService>()
                    .SetupSequence(scoringService => scoringService.CalculateScore(It.IsAny<List<Card>>()))
                    .Returns(21)
                    .Returns(7)
                    .Returns(21)
                    .Returns(7);

            mocker.GetMock<IIO>().Setup(io => io.GetPlayerChoice()).Returns("stay");

            // Mock ResetGame to prevent it from interfering with the test
            mocker.GetMock<IPlayer>().Setup(player => player.CardsInHand).Returns(new List<Card>());
            mocker.GetMock<IPlayer>().Setup(dealer => dealer.CardsInHand).Returns(new List<Card>());
            mocker.GetMock<IDeck>().Setup(deck => deck.deckOfCards).Returns(new List<Card>());

          //Act
            controller.Run();

          //Assert
            mocker.GetMock<IIO>().Verify(io => io.AnnounceWinner("Player", "Dealer"), Times.Once);

        
        }


        [Fact]
        public void Dealer_Should_Hit_When_Score_Is_Less_Than_Equal_To_17()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            mocker.GetMock<IScoringService>().SetupSequence(scoringService => scoringService.CalculateScore(It.IsAny<List<Card>>())).Returns(12).Returns(16).Returns(19);

            // Act
            controller.PlayDealerTurn();

            // Assert
            mocker.GetMock<IPlayer>().Verify(dealer => dealer.ReceiveCards(It.IsAny<List<Card>>()), Times.AtLeastOnce());
        }

        [Fact]
        public void Dealer_Should_Stay_When_Score_Is_Greater_Than_17()
        {
            //Arrange
            var(controller,mocker) = CreateGameController();
            mocker.GetMock<IScoringService>().Setup(scoringService => scoringService.CalculateScore(It.IsAny<List<Card>>())).Returns(19);

            //Act
            controller.PlayDealerTurn();

            //Assert
            mocker.GetMock<IPlayer>().Verify(dealer => dealer.ReceiveCards(It.IsAny<List<Card>>()), Times.Never());
        }

        [Fact]

        public void Dealer_Should_Bust_When_Score_Greater_Than_21()
        {
            //Arrange
            var(controller,mocker) = CreateGameController();
            mocker.GetMock<IScoringService>()
            .SetupSequence(scoringService => scoringService.CalculateScore(It.IsAny<List<Card>>()))
            .Returns(15) 
            .Returns(23);

            //Act
            controller.PlayDealerTurn();

            //Assert
            controller.GameState.IsDealerBust.Should().BeTrue();
        }

        [Fact]
        public void Dealer_Should_Win_When_Score_Is_21()
        {
            //Arrange
            var(controller,mocker) = CreateGameController();
            mocker.GetMock<IScoringService>()
            .SetupSequence(scoringService => scoringService.CalculateScore(It.IsAny<List<Card>>()))
            .Returns(15) 
            .Returns(21);

            //Act
            controller.PlayDealerTurn();

            //Assert
            controller.GameState.IsDealerWinner.Should().BeTrue();
        }
        [Fact]
        public void ResetGame_Should_Reset_Game()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            // fake a non-empty game state
            mocker.GetMock<IPlayer>().Setup(player => player.CardsInHand).Returns(new List<Card> { new Card(Suit.spade, Rank.five) });
            mocker.GetMock<IPlayer>().Setup(dealer => dealer.CardsInHand).Returns(new List<Card> { new Card(Suit.heart, Rank.king) });
            controller.GameState.IsPlayerBust = true;
            controller.GameState.IsDealerBust = true;
            mocker.GetMock<IDeck>().Setup(deck => deck.deckOfCards).Returns(new List<Card> { new Card(Suit.diamond, Rank.two) });

            // Act
            controller.ResetGame();

            // Assert
            mocker.GetMock<IPlayer>().Verify(player => player.Reset(), Times.Exactly(2));
            controller.GameState.IsPlayerBust.Should().BeFalse("Player bust should be reset");
            controller.GameState.IsDealerBust.Should().BeFalse("Dealer bust should be reset");
            mocker.GetMock<IDeck>().Object.deckOfCards.Should().BeEmpty("Deck should be cleared");
        }

        [Fact]
        public void Run_Should_Announce_Player_Win_When_Dealer_Busts()
        {
            var (controller, mocker) = CreateGameController();

            mocker.GetMock<IIO>().Setup(io => io.GetPlayerChoice()).Returns("stay");

            mocker.GetMock<IScoringService>()
                .SetupSequence(s => s.CalculateScore(It.IsAny<List<Card>>()))
                .Returns(10) // Player score after initial cards
                .Returns(22) // Dealer score after initial cards
                .Returns(10) // Player score for the next call
                .Returns(22); // Dealer score for the next call

            // Mock ResetGame to prevent it from interfering with the test
            mocker.GetMock<IPlayer>().Setup(player => player.CardsInHand).Returns(new List<Card>());
            mocker.GetMock<IPlayer>().Setup(dealer => dealer.CardsInHand).Returns(new List<Card>());
            mocker.GetMock<IDeck>().Setup(deck => deck.deckOfCards).Returns(new List<Card>());

            controller.Run();

            mocker.GetMock<IIO>().Verify(io => io.AnnounceWinner("Player", "Dealer"), Times.Once);
        }

        [Fact]
        public void Game_Should_Draw_If_Both_Get_Same_Score_And_Decided_To_Stay()
        {
            var (controller, mocker) = CreateGameController();

            mocker.GetMock<IIO>().Setup(io => io.GetPlayerChoice()).Returns("stay");

            mocker.GetMock<IScoringService>()
                .SetupSequence(s => s.CalculateScore(It.IsAny<List<Card>>()))
                .Returns(18) // Player score after initial cards
                .Returns(18) // Dealer score after initial cards
                .Returns(18) // Player score for the next call
                .Returns(18) // Dealer score for the next call
                .Returns(18) // Player score for the next call
                .Returns(18) // Dealer score for the next call
                .Returns(18); // Player score for the next call

            //  Mock ResetGame to prevent it from interfering with the test
            mocker.GetMock<IPlayer>().Setup(player => player.CardsInHand).Returns(new List<Card>());
            mocker.GetMock<IPlayer>().Setup(dealer => dealer.CardsInHand).Returns(new List<Card>());
            mocker.GetMock<IDeck>().Setup(deck => deck.deckOfCards).Returns(new List<Card>());


            controller.Run();

            mocker.GetMock<IIO>().Verify(io => io.DrawGame(), Times.Once);
        }

    }
}
