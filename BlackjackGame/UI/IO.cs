using System;
using System.Collections.Generic;
using BlackjackGame.Constants;
using BlackjackGame.Interfaces;
using BlackjackGame.Models;
using BlackjackGame.Enums;

namespace BlackjackGame.UI
{
    public class IO : IIO
    {
        public void Welcome()
        {
            Console.WriteLine(PromptMessages.Welcome);
        }

        public string GetPlayerChoice()
        {
            Console.WriteLine(PromptMessages.HitOrStay);
            string playerChoice = Console.ReadLine().ToLower();
            if (playerChoice == "hit") return playerChoice;
            return "stay";
        }

        public void DisplayScore(string player, int score)
        {
            Console.WriteLine($"{player} score is {score}");
        }

        public void AnnounceWinner(string winner, string loser)
        {
            if (winner == "player")
                Console.WriteLine(PromptMessages.PlayerWins);
            else
                Console.WriteLine(PromptMessages.DealerWins);
        }

        public void DisplayPlayerHand(List<Card> playerHand)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Enable emojis

            // Header
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nYour Hand:");
            Console.WriteLine($"{"Rank",-6} | {"Suit",-10}");
            Console.WriteLine(new string('-', 24));
            Console.ResetColor();

            // Loop through cards
            foreach (var card in playerHand)
            {
                string emoji = "";

                switch (card.Suit)
                {
                    case Suit.heart:
                        emoji = "♥️";
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case Suit.diamond:
                        emoji = "♦️";
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case Suit.club:
                        emoji = "♣️";
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case Suit.spade:
                        emoji = "♠️";
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    default:
                        emoji = "";
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }

                Console.WriteLine($"{card.Rank,-6} | {card.Suit,-10} {emoji}");
            }

            Console.ResetColor(); // Reset after full hand is printed
        }

        public void DisplayDealerHand(List<Card> dealerHand)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Enable emojis

    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("\nDealer's Hand:");
    Console.ResetColor();

    // Header
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine($"{"Rank",-6} | {"Suit",-10}");
    Console.WriteLine(new string('-', 24));
    Console.ResetColor();

    foreach (var card in dealerHand)
    {
        string emoji = "";

        switch (card.Suit)
        {
            case Suit.heart:
                emoji = "♥️";
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case Suit.diamond:
                emoji = "♦️";
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case Suit.club:
                emoji = "♣️";
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case Suit.spade:
                emoji = "♠️";
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
            default:
                emoji = "";
                Console.ForegroundColor = ConsoleColor.Gray;
                break;
        }

        Console.WriteLine($"{card.Rank,-6} | {card.Suit,-10} {emoji}");
    }

    Console.ResetColor();
        }
    }
}
