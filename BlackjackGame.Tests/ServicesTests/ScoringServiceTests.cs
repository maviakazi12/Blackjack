using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Enums;
using BlackjackGame.Models;
using BlackjackGame.Services;
using FluentAssertions;
using Xunit;

namespace BlackjackGame.Tests.ServicesTests
{
    public class ScoringServiceTests
    {
        [Fact]
        public void CalculateScore_Should_Return_Zero_For_Empty_Hand()
        {   //Arrange
            ScoringService scoring = new ScoringService();
            List<Card> hand = new List<Card>();

            //Act
            int score = scoring.CalculateScore(hand);
            
            //Assert
            score.Should().Be(0);
        }

        [Fact]
        public void CalculateScore_Should_Return_14_For_Given_Hand()
        {   //Arrange
            ScoringService scoring = new ScoringService();
            List<Card> hand = new List<Card>{
                new Card (Suit.spade, Rank.four),
                new Card (Suit.diamond, Rank.ten),
            };

            //Act
            int score = scoring.CalculateScore(hand);
            
            //Assert
            score.Should().Be(14);
        }

        [Theory]
        [InlineData(Rank.jack, 10)]
        [InlineData(Rank.queen, 10)]
        [InlineData(Rank.king, 10)]

        public void CalculateScore_Should_Return_10_For_Any_Face_Value_Card(Rank cardRank, int value){

            //Arrange
            ScoringService scoring = new ScoringService();
            List<Card> hand = new List<Card>{
                new Card (Suit.spade, cardRank)};
            
            //Act
            int score = scoring.CalculateScore(hand);

            //Assert
            score.Should().Be(value);
        }

        [Fact]
        public void CalculateScore_Should_Sum_Values_Of_Mixed_Cards(){

            //Arrange

            ScoringService scoring = new ScoringService();
            List<Card> hand = new List<Card>{
                new Card (Suit.spade, Rank.jack),
                new Card (Suit.spade, Rank.king),
                new Card (Suit.spade, Rank.two)
                };
            //Act
            int score = scoring.CalculateScore(hand);

            //Assert
            score.Should().Be(22);
        }

        [Fact]
        public void CalculateScore_Should_Treat_Ace_As_11_When_Safe(){

            //Arrange
            ScoringService scoring = new ScoringService();
            List<Card> hand = new List<Card>{
                new Card (Suit.spade, Rank.ace),
                new Card (Suit.spade, Rank.two),
                new Card (Suit.diamond, Rank.two)
                };
            //Act
            int score = scoring.CalculateScore(hand);

            //Assert
            score.Should().Be(15);
        }

        [Fact]
        public void CalculateScore_Should_Treat_Ace_As_1_When_Total_Would_Bust(){

            //Arrange
            ScoringService scoring = new ScoringService();
            List<Card> hand = new List<Card>{
                new Card (Suit.spade, Rank.ace),
                new Card (Suit.spade, Rank.two),
                new Card (Suit.diamond, Rank.jack)
                };

            //Act
            int score = scoring.CalculateScore(hand);

            //Assert
            score.Should().Be(13);
        }

        [Fact]
        public void CalculateScore_Should_Handle_Multiple_Aces_In_Hand_Without_Exceeding_TwentyOne(){

            //Arrange
            ScoringService scoring = new ScoringService();
            List<Card> hand = new List<Card>{
                new Card (Suit.spade, Rank.ace),
                new Card (Suit.spade, Rank.jack),
                new Card (Suit.diamond, Rank.ace)
                };

            //Act
            int score = scoring.CalculateScore(hand);

            //Assert
            score.Should().Be(12);
        }
    }
}