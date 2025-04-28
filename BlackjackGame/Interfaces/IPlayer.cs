using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Models;

namespace BlackjackGame.Interfaces
{
    public interface IPlayer
    {
        public List<Card> CardsInHand { get; set;}

        public void ReceiveCards(List<Card> cards);
    }
}