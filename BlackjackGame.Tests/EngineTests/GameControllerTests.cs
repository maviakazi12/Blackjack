using BlackjackGame.Engine;
using BlackjackGame.Interfaces;
using BlackjackGame.Enums;
using BlackjackGame.Models;
using Moq;
using FluentAssertions;

namespace BlackjackTests.EngineTests
{
    public class GameControllerTests
    {
        [Fact]
        public void StartGame_Should_Call_InitializeDeck_Method()
        {
            // Arrange
            var mockDeck = new Mock<IDeck>();
            GameController gameController = new GameController(mockDeck.Object, Mock.Of<IPlayer>(), Mock.Of<IPlayer>(), 2, Mock.Of<IIO>(), Mock.Of<IScoringService>());

            // Act
            gameController.Run();

            // Assert
            mockDeck.Verify(deck => deck.InitializeDeck(), Times.Once);

        }

        [Fact]
        public void StartGame_Should_Call_Shuffle_Method()
        {
            // Arrange
            var mockDeck = new Mock<IDeck>();
            GameController gameController = new GameController(mockDeck.Object, Mock.Of<IPlayer>(), Mock.Of<IPlayer>(), 2, Mock.Of<IIO>(), Mock.Of<IScoringService>());

            // Act
            gameController.Run();

            // Assert
            mockDeck.Verify(deck => deck.Shuffle(), Times.Once);

        }

        [Fact]
        public void StartGame_Should_Call_Draw_2_Times_For_Player_And_Dealer()
        {
            // Arrange
            var mockDeck = new Mock<IDeck>();
            GameController gameController = new GameController(mockDeck.Object, Mock.Of<IPlayer>(), Mock.Of<IPlayer>(), 2, Mock.Of<IIO>(), Mock.Of<IScoringService>());

            // Act
            gameController.Run();

            // Assert
            mockDeck.Verify(deck => deck.Draw(2), Times.Exactly(2));
        }

        [Fact]
        public void StartGame_Should_Deal_n_Number_Of_Cards()
        {
            // Arrange
            var mockDeck = new Mock<IDeck>();
            GameController gameController = new GameController(mockDeck.Object, Mock.Of<IPlayer>(), Mock.Of<IPlayer>(), 4, Mock.Of<IIO>(), Mock.Of<IScoringService>());

            // Act
            gameController.Run();

            // Assert
            mockDeck.Verify(deck => deck.Draw(4), Times.Exactly(2));
        }

        [Fact]
        public void Constructor_Should_Throw_If_Deck_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            new GameController(null, Mock.Of<IPlayer>(), Mock.Of<IPlayer>(), 2, Mock.Of<IIO>(), Mock.Of<IScoringService>()));
        }

        [Fact]
        public void Run_Should_Call_ReceiveCards_On_Player_And_Dealer_With_Deck_DrawnCards()
        {
            // Arrange
            var mockDeck = new Mock<IDeck>();
            var mockPlayer = new Mock<IPlayer>();
            var mockDealer = new Mock<IPlayer>();
            GameController gameController = new GameController(mockDeck.Object, mockPlayer.Object, mockDealer.Object, 4, Mock.Of<IIO>(), Mock.Of<IScoringService>());
            var drawnCards = new List<Card>
            { new Card(Suit.heart, Rank.ace),
                new Card(Suit.spade, Rank.ten)
            };
            mockDeck.Setup(deck => deck.drawnCards).Returns(drawnCards);

            // Act
            gameController.Run();

            //Assert
            mockPlayer.Verify(player => player.ReceiveCards(mockDeck.Object.drawnCards));
            mockDealer.Verify(dealer => dealer.ReceiveCards(mockDeck.Object.drawnCards));
        }

        [Fact]
        public void PlayPlayerTurn_Should_Call_ReceiveCards_On_Player_They_Choose_Hit()
        {
            //Arrange
            var mockIO = new Mock<IIO>();
            var mockDeck = new Mock<IDeck>();
            var mockPlayer = new Mock<IPlayer>();
            var mockDealer = new Mock<IPlayer>();
            GameController gameController = new GameController(mockDeck.Object, mockPlayer.Object, mockDealer.Object, 2, mockIO.Object, Mock.Of<IScoringService>());
            var drawnCard = new List<Card>
            { new Card(Suit.heart, Rank.ace)};

            mockDeck.Setup(d => d.drawnCards).Returns(drawnCard);
            mockIO.Setup(a => a.GetPlayerChoice()).Returns("hit");

            //Act
            gameController.PlayPlayerTurn();

            //Assert
            mockPlayer.Verify(player => player.ReceiveCards(drawnCard), Times.Once);
        }

        [Fact]
        public void PlayPlayerTurn_Should_Not_Draw_Card_When_Player_Chooses_Stay()
        {
            //Arrange
            var mockIO = new Mock<IIO>();
            var mockDeck = new Mock<IDeck>();
            var mockPlayer = new Mock<IPlayer>();
            var mockDealer = new Mock<IPlayer>();
            GameController gameController = new GameController(mockDeck.Object, mockPlayer.Object, mockDealer.Object, 2, mockIO.Object, Mock.Of<IScoringService>());
            mockIO.Setup(a => a.GetPlayerChoice()).Returns("stay");

            //Act
            gameController.PlayPlayerTurn();

            //Assert
            mockPlayer.Verify(player => player.ReceiveCards(It.IsAny<List<Card>>()), Times.Never());
        }

        [Fact]
        public void PlayPlayerTurn_Should_Set_GameState_To_Dealer_After_Hit_When_Not_Bust_Or_Win()
        {
            //Arrange
            var mockIO = new Mock<IIO>();
            var mockDeck = new Mock<IDeck>();
            var mockPlayer = new Mock<IPlayer>();
            var mockDealer = new Mock<IPlayer>();
            GameController gameController = new GameController(mockDeck.Object, mockPlayer.Object, mockDealer.Object, 2, mockIO.Object, Mock.Of<IScoringService>());
            mockIO.Setup(a => a.GetPlayerChoice()).Returns("hit");

            //Act
            gameController.PlayPlayerTurn();

            //Assert
            gameController.GameState.CurrentTurn.Should().Be(PlayerType.Dealer);
        }

    }
}