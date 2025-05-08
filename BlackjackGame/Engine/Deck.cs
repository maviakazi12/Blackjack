using BlackjackGame.Enums;
using BlackjackGame.Models;
using BlackjackGame.Interfaces;

namespace BlackjackGame.Engine
{
    public class Deck : IDeck
    {
        public List<Card> deckOfCards { get; private set; } = new List<Card>();
        public List<Card> drawnCards { get; private set; } = new List<Card>();
        private bool isDeckInitialized { get; set; }

        public Deck()
        {
            isDeckInitialized = false;
        }

        public void InitializeDeck()
        {
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank r in Enum.GetValues(typeof(Rank)))
                {
                    deckOfCards.Add(new Card(s, r));
                }
            }
            isDeckInitialized = true;
        }

        public void Shuffle()
        {
            if (!isDeckInitialized || deckOfCards.Count == 0)
            {
                throw new Exception("Deck is not present");
            }
            Random rand = new Random();

            for (int i = deckOfCards.Count - 1; i > 0; i--)
            {

                var j = rand.Next(i + 1);
                var temp = deckOfCards[i];
                deckOfCards[i] = deckOfCards[j];
                deckOfCards[j] = temp;
            }
        }

        public void Draw(int numberOfCardsToDraw)
        {
            if (!isDeckInitialized)
            {
                throw new Exception("Deck is not initialized");
            }
            if (drawnCards.Count > 0) { drawnCards.Clear(); }
            
            if (deckOfCards.Count == 0 ){
                throw new InvalidOperationException("Cannot draw a card. The deck is empty.");
            }

            Stack<Card> stackedCards = new Stack<Card>(deckOfCards);

            for (int i = 0; i < numberOfCardsToDraw; i++)
            {
                Card card = stackedCards.Pop();
                drawnCards.Add(card);
            }
            deckOfCards = stackedCards.ToList();


        }
    }
}