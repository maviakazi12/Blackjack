using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            int score = scoring.CalculateSCore(hand);
            
            //Assert
            score.Should().Be(0);
        }
    }
}