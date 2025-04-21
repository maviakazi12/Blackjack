using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BlackjackGame.Engine;
using Moq;

namespace BlackjackTests.EngineTests
{
    public class EngineTests
    {
        [Fact]
        public void StartGame_Should_Initialize_The_Deck()
        {
            // Arrange
            var mockDeck = new Mock<IDeck>();
            
            // Act
            Engine.StartGame(mockDeck.Object);

            // Assert
            mockDeck.Verify(deck=> deck.InitializeDeck(), Times.Once);
        }
    }
}