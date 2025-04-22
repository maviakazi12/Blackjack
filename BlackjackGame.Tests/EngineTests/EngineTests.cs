using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
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
            mockDeck.Verify(deck=> deck.InitializeDeck(), Times.Once);

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
            mockDeck.Verify(deck=> deck.Shuffle(), Times.Once);

        }
    }
}