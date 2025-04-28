using BlackjackGame.Enums;

namespace BlackjackGame.Models
{
    public class GameState
{
    public bool IsGameOver { get; set; }
    public PlayerType Winner { get; set; }
    public bool IsPlayerBust { get; set; }
    public bool IsDealerBust { get; set; }
    public PlayerType CurrentTurn { get; set; }
}
}