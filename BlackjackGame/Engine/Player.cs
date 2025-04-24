using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Interfaces;
using BlackjackGame.Models;

namespace BlackjackGame.Engine
{
    public class Player :IPlayer
    {
        public List<Card> CardsInHand { get; set;} = new List<Card>();

        public void ReceiveCards(List<Card> cards){
            CardsInHand.AddRange(cards);
        }
    }
}