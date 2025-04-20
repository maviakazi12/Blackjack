using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using BlackjackGame.Engine;



namespace BlackjackGameTests.EngineTests
{
    public class DeckTests
    {
        [Fact]
        public void Deck_Should_Contain_52_Cards()
        {
            // Arrange
            var deck = new Deck();

            // Act
            var cardDeck = deck.deckOfCards;

            // Assert
            cardDeck.Should().HaveCount(52);
        }

        [Fact]
        public void Deck_Should_Have_Unique_Cards()
        {
            // Arrange
            var deck = new Deck();

            // Act
            var cardDeck = deck.deckOfCards;

            // Assert
            cardDeck.Select(card => (card.Suit, card.Rank)).Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Shuffled_Cards_Should_Not_Be_Same_As_Original_Cards()
        {
            // Arrange
            var deck = new Deck();
            var originalDeck = deck.deckOfCards.ToList();

            // Act
            deck.Shuffle();
            var shuffledDeck = deck.deckOfCards.ToList();

            // Assert
            shuffledDeck.Should().NotEqual(originalDeck);
        }

        [Fact]
        public void Shuffled_Cards_Should_Contain_52_Cards()
        {
            // Arrange
            var deck = new Deck();

            // Act
            deck.Shuffle();
            var shuffledDeck = deck.deckOfCards.ToList();

            // Assert
            shuffledDeck.Should().HaveCount(52);
        }

    [Fact]
        public void Shuffled_Deck_Should_Have_Unique_Cards()
        {
            // Arrange
            var deck = new Deck();

            // Act
            deck.Shuffle();
            var shuffledDeck = deck.deckOfCards.ToList();

            // Assert
            shuffledDeck.Select(card => (card.Suit, card.Rank)).Should().OnlyHaveUniqueItems();
        }

    }

}
