using BlackjackGame.Models;

namespace BlackjackGame.Interfaces
{
    public interface IScoringService
    {
        public int CalculateScore(List<Card> cards);
    }
}