using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using BlackjackGame.Engine;
using BlackjackGame.Models;
using BlackjackGame.Enums;



namespace BlackjackGameTests.EngineTests
{
    public class DeckTests
    {
        Deck deck = new Deck();
        
        [Fact]
        public void Deck_Should_Contain_52_Cards()
        {
             // Act
        [Fact]
        public void Deck_Should_Contain_52_Cards()
        {
            // Arrange
            var deck = new Deck();

            // Act
>>>>>>> 307a1f9 (created the deck)
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
            // Act
            deck.Shuffle();
            var shuffledDeck = deck.deckOfCards.ToList();

            // Assert
            shuffledDeck.Should().HaveCount(52);
        }

        [Fact]
        public void Shuffled_Deck_Should_Have_Unique_Cards()
        {
            // Act
            deck.Shuffle();
            var shuffledDeck = deck.deckOfCards.ToList();

            // Assert
            shuffledDeck.Select(card => (card.Suit, card.Rank)).Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Draw_Method_Should_Draw_One_Card()
        {
            // Act
            deck.Shuffle();
            deck.Draw();
            var result = deck.drawnCard;
            var singleCard = result.Single();

            // Assert
            singleCard.Rank.Should().NotBe((Rank)0);
            singleCard.Suit.Should().NotBe((Suit)0);
        }

        [Fact]
        public void Draw_Method_Should_Reduce_Deck_Size_By_1()
        {
            // Arrange
            deck.Shuffle();

            // Act
            deck.Draw();

            // Assert
            deck.deckOfCards.Should().HaveCount(51);
            
        }

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
