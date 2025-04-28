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

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.InitializeDeck(), Times.Once);
        }

        [Fact]
        public void dealInitialHands_Should_Call_Shuffle_Method()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.Shuffle(), Times.Once);
        }

        [Fact]
        public void DealInitialHands_Should_Call_Draw_2_Times_For_Player_And_Dealer()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.Draw(2), Times.Exactly(2));
        }

        [Fact]
        public void dealInitialHands_Should_Deal_n_Number_Of_Cards()
        {
            // Arrange
            var (controller, mocker) = CreateGameController(4);

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.Draw(4), Times.Exactly(2));
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
        public void Run_Should_Call_ReceiveCards_On_Player_And_Dealer_With_Deck_DrawnCards()
        {
            // Arrange
            var (controller, mocker) = CreateGameController(4);

            var drawnCards = new List<Card>
            {
                new Card(Suit.heart, Rank.ace),
                new Card(Suit.spade, Rank.ten)
            };

            mocker.GetMock<IDeck>().Setup(deck => deck.drawnCards).Returns(drawnCards);

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IPlayer>().Verify(player => player.ReceiveCards(drawnCards));
            mocker.GetMock<IPlayer>().Verify(dealer => dealer.ReceiveCards(drawnCards));
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
public void Reset_Should_Reset_Game()
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
    mocker.GetMock<IPlayer>().Object.CardsInHand.Should().BeEmpty("Player's hand should be cleared");
    mocker.GetMock<IPlayer>().Object.CardsInHand.Should().BeEmpty("Dealer's hand should be cleared");
    controller.GameState.IsPlayerBust.Should().BeFalse("Player bust should be reset");
    controller.GameState.IsDealerBust.Should().BeFalse("Dealer bust should be reset");
    mocker.GetMock<IDeck>().Object.deckOfCards.Should().BeEmpty("Deck should be cleared");
}

    }
}
