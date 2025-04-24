using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Interfaces;
using BlackjackGame.Models;

namespace BlackjackGame.Services
{
    public class ScoringService : IScoringService
    {
        public int CalculateScore(List<Card> cards){
            
            if (cards.Count == 0) return 0;
            int totalScore = 0;
            foreach (Card card in cards){
                totalScore += (int)card.Rank;
            }
            return totalScore;
        }
    }
}