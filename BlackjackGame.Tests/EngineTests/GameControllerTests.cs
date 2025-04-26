using BlackjackGame.Engine;
using BlackjackGame.Interfaces;
using BlackjackGame.Enums;
using BlackjackGame.Models;
using Moq;
using Moq.AutoMock;
using FluentAssertions;

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
        public void StartGame_Should_Call_InitializeDeck_Method()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.InitializeDeck(), Times.Once);
        }

        [Fact]
        public void StartGame_Should_Call_Shuffle_Method()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.Shuffle(), Times.Once);
        }

        [Fact]
        public void StartGame_Should_Call_Draw_2_Times_For_Player_And_Dealer()
        {
            // Arrange
            var (controller, mocker) = CreateGameController();

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.Draw(2), Times.Exactly(2));
        }

        [Fact]
        public void StartGame_Should_Deal_n_Number_Of_Cards()
        {
            // Arrange
            var (controller, mocker) = CreateGameController(4);

            // Act
            controller.Run();

            // Assert
            mocker.GetMock<IDeck>().Verify(deck => deck.Draw(4), Times.Exactly(2));
        }

        [Fact]
        public void Constructor_Should_Throw_If_Deck_Is_Null()
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
            mocker.GetMock<IIO>().Setup(io => io.GetPlayerChoice()).Returns("hit");

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

            mocker.GetMock<IIO>().Setup(io => io.GetPlayerChoice()).Returns("hit");

            // Act
            controller.PlayPlayerTurn();

            // Assert
            controller.GameState.CurrentTurn.Should().Be(PlayerType.Dealer);
        }
    }
}
