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
        private Deck deck;

        public DeckTests()
        {
            deck = new Deck();
            deck.InitializeDeck();
        }


        [Fact]
        public void Deck_Should_Contain_52_Cards()
        {
            // Act
            var cardDeck = deck.deckOfCards;

            // Assert
            cardDeck.Should().HaveCount(52);
        }

        [Fact]
        public void Deck_Should_Have_Unique_Cards()
        {
            // Act

            var cardDeck = deck.deckOfCards;

            // Assert
            cardDeck.Select(card => (card)).Should().OnlyHaveUniqueItems();
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
            shuffledDeck.Select(card => card).Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Draw_Method_Should_Draw_One_Card_When_Given_1_parameter()
        {
            // Act
            deck.Shuffle();
            deck.Draw(1);
            var result = deck.drawnCards;
            var singleCard = result.Single();

            // Assert
            Enum.IsDefined(typeof(Suit), singleCard.Suit).Should().BeTrue();
            Enum.IsDefined(typeof(Rank), singleCard.Rank).Should().BeTrue();
        }

        [Fact]
        public void Draw_Method_Should_Reduce_Deck_Size_By_1()
        {
            // Arrange
            deck.Shuffle();

            // Act
            deck.Draw(1);

            // Assert
            deck.deckOfCards.Should().HaveCount(51);
        }

        [Fact]
        public void DrawnCards_List_Should_Be_Cleared_When_Draw_Is_Called_After_Another_Call()
        {
            // Arrange
            deck.Shuffle();

            // Act
            deck.Draw(2);
            deck.Draw(4);

            // Assert
            deck.drawnCards.Should().HaveCount(4);
        }

        [Fact]
        public void Should_Throw_Exception_When_Deck_Is_Empty_And_Draw_Is_Called()
        {
            // Arrange
            deck.deckOfCards.Clear();
            // Act
            Action act = () => deck.Draw(2); 
            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot draw a card. The deck is empty.");
        }

        [Fact]
        public void Should_Throw_Exception_When_Deck_Is_Empty_And_Shuffle_Is_Called()
        {
            // Arrange
            deck.deckOfCards.Clear();
            // Act
            Action act = () => deck.Shuffle(); 
            // Assert
            act.Should().Throw<Exception>().WithMessage("Deck is not present");
        }
    }
}
