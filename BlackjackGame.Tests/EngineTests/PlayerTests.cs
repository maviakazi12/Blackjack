using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BlackjackGame.Engine;
using BlackjackGame.Interfaces;
using BlackjackGame.Enums;
using BlackjackGame.Models;
using BlackjackGame.Tests.Fakes;
using FluentAssertions;
using Moq;


namespace BlackjackGame.Tests.EngineTests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_Class_Should_Receive_Initial_Unique_Cards_From_GameController()
        {
            // Arrange
            Player player = new Player();
            var fakeGameController = new FakeGameController();
            
            //Act
            fakeGameController.StartGame(player);

            //Assert
            player.CardsInHand.Should().HaveCount(2);
            player.CardsInHand.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Player_Class_Should_Have_Empty_Hand_On_Initialization()
        {
            //Arrange
            Player player = new Player();

            player.CardsInHand.Should().HaveCount(0);
        }

        [Fact]
        public void Player_Should_Receive_A_Card_When_Given_One()
        {
            //Arrange
            Player player = new Player();
            var fakeGameController = new FakeGameController();

            //Act
            fakeGameController.DealCard(player);

            //Assert
            player.CardsInHand.Should().HaveCount(1);
        }

        [Fact]
        public void Player_Should_Receive_A_Card_When_They_Choose_Hit()
        {
            //Arrange
            var mockIO = new Mock<IIO>();
            mockIO.Setup(a=>a.PlayerInput()).Returns("hit");
            Player player = new Player(mockIO.Object);
            var initialCount = player.CardsInHand.Count;
            //Act
            player.HitOrStay();
            
            //Assert
            player.CardsInHand.Count.Should().Be(initialCount+1);
        }
    }
    
}