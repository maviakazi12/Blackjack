using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackjackGame.Enums;
using BlackjackGame.Models;
using BlackjackGame.Interfaces;

namespace BlackjackGame.Engine
{
    public class Deck : IDeck
    {
        public List<Card> deckOfCards { get; private set; }
        public List<Card> drawnCards { get; private set; }
        private bool isDeckInitialized { get; set; }

        public Deck()
        {
            isDeckInitialized = false;
        }

        public void InitializeDeck()
        {
            deckOfCards = new List<Card>();
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
            if (!isDeckInitialized)
            {
                throw new Exception("Deck is not initialized");
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

            drawnCards ??= new List<Card>();
            drawnCards.Clear();
            Stack<Card> stackedCards = new Stack<Card>(deckOfCards);

            for (int i = 0; i <= numberOfCardsToDraw; i++)
            {
                Card card = stackedCards.Pop();
                drawnCards.Add(card);
            }

            // var drawnCardFromStack = stackedCards.Pop();
            // drawnCards = new List<Card> { drawnCardFromStack };
            // deckOfCards = stackedCards.ToList();


        }
    }
}