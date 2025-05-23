using BlackjackGame.Interfaces;
using BlackjackGame.Models;

namespace BlackjackGame.Engine
{
    public class Dealer :IPlayer
    {
        public List<Card> CardsInHand { get; set;} = new List<Card>();

        public void ReceiveCards(List<Card> cards){
            CardsInHand.AddRange(cards);
        }

        public void Reset()
        {
            CardsInHand.Clear();
        }
        
    }
}