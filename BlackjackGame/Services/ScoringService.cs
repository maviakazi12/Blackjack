using BlackjackGame.Interfaces;
using BlackjackGame.Models;
using BlackjackGame.Enums;

namespace BlackjackGame.Services
{
    public class ScoringService : IScoringService
    {
        public int CalculateScore(List<Card> cards){
            
            if (cards == null || cards.Count == 0) return 0;
            int totalScore = 0;
            int aceCount = 0;
            foreach (Card card in cards){
                if (card.Rank == Rank.ace)aceCount++;
                totalScore += (int)card.Rank;
            }
            while(totalScore>21 && aceCount>0) {
                totalScore-=10;
                aceCount --;
            }
            return totalScore;
        }
    }
}