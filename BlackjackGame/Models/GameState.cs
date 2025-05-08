using BlackjackGame.Enums;

namespace BlackjackGame.Models
{
    public class GameState
{
    public PlayerType Winner { get; set; }
    public bool IsPlayerBust { get; set; }
    public bool IsDealerBust { get; set; }
    public bool IsDealerWinner { get; set; }
    public bool IsPlayerWinner { get; set; }
}
}