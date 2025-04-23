using BlackjackGame.Engine;
using BlackjackGame.Interfaces;

using Moq;

namespace BlackjackTests.EngineTests
{
    public class EngineTests
    {
        [Fact]
        public void StartGame_Should_Call_InitializeDeck_Method()
        {
            // Arrange
            var mockDeck = new Mock<IDeck>();
            Engine engine = new Engine(mockDeck.Object);

            // Act
            engine.StartGame();

            // Assert
            mockDeck.Verify(deck => deck.InitializeDeck(), Times.Once);

        }

        [Fact]
        public void StartGame_Should_Call_Shuffle_Method()
        {
            // Arrange
            var mockDeck = new Mock<IDeck>();
            Engine engine = new Engine(mockDeck.Object);

            // Act
            engine.StartGame();

            // Assert
            mockDeck.Verify(deck => deck.Shuffle(), Times.Once);

        }

        // [Fact]
        // public void DealCards_Should_Call_Draw_Method_Twice()
        // {
        //     // Arrange
        //     var mockDeck = new Mock<IDeck>();
        //     var card = new Card(Suit.heart, Rank.two);
        //     mockDeck.Setup(deck => deck.drawnCards).Returns(new List<Card> { card });
        //     Engine engine = new Engine(mockDeck.Object);

        //     // Act
        //     engine.DealCards();

        //     // Assert
        //     mockDeck.Verify(deck => deck.Draw(1), Times.Exactly(2));

        // }
    }
}