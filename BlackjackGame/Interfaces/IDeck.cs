using BlackjackGame.Models;

namespace BlackjackGame.Interfaces
{
    public interface IDeck
    {
        public List<Card> deckOfCards { get; }
        public List<Card> drawnCards { get; }
        void InitializeDeck();
        void Shuffle();
        void Draw(int numberOfCardsToDraw);
    }
}