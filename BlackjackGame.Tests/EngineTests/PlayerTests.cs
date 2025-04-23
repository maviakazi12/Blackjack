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


namespace BlackjackGame.Tests.EngineTests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_Class_Should_Receive_Initial_Cards_From_GameController()
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
    }
}