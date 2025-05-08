using BlackjackGame.Engine;
using BlackjackGame.Enums;
using BlackjackGame.Models;

namespace BlackjackGame.Tests.Fakes
{
    public class FakeGameController
    {
        public void StartGame(Player player)
        {
            var _drawnCards = new List<Card>{
                new Card(Suit.spade, Rank.two),
                new Card(Suit.heart, Rank.three),
            };
            player.ReceiveCards(_drawnCards);
        }

        public void DealCard(Player player){
            var _drawnCard = new List<Card>{
                new Card(Suit.spade, Rank.two)
            };
            player.ReceiveCards(_drawnCard);
        }
    }
}