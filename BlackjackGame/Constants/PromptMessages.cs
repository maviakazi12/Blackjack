namespace BlackjackGame.Constants
{
    public static class PromptMessages
    {
        public const string Welcome = "====================================\n" +
        "     🃏 Welcome to Blackjack! 🃏     \n" +
        "====================================\n" +
        "Try your luck, test your strategy, and aim for 21!\n" +
        "Type \"hit\" to take a card, or \"stay\" to hold your hand.\n" +
        "Good luck, and may the best hand win!\n";
        public const string HitOrStay = "Would you like to hit or stay?";
        public const string InvalidInput = "Please enter a valid choice.";
        public const string DealerHit = "Dealer has decided to hit!";
        public const string DealerStay = "Dealer has decided to Stay, your turn!";
        public const string Draw = "Whoa! It's a stand-off. Dealer and player are equally awesome today!";
        public const string PlayerWins = "🎉 Dealer’s crying in the corner... You win!";
        public const string DealerWins = "🧊 Cold and calculated — dealer takes the win!";
    }
}