using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Models;

namespace BlackjackGame.Interfaces
{
    public interface IIO
    {
        public string GetPlayerChoice();
        public void DisplayScore(string player, int score);
        public void AnnounceWinner(string winner, string loser);
        public void DisplayPlayerHand(List<Card> playerHand);
        public void DisplayDealerHand(List<Card> dealerHand);
        public void DisplayDealerChoice(string dealerChoice);
        public void DrawGame();

    }
}