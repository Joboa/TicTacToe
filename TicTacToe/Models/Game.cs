using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TicTacToe.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }
        [ForeignKey("Player1Id")]
        [InverseProperty("GamesAsPlayer1")]
        public Player Player1 { get; set; }
        public int Player2Id { get; set; }
        [ForeignKey("Player2Id")]
        [InverseProperty("GamesAsPlayer2")]
        public Player Player2 { get; set; }
        public string CurrentPlayer { get; set; }
        public string Winner { get; set; } = string.Empty;
        [JsonIgnore]
        public string[][] GameBoard { get; set; }
        public string GameBoardJson
        {
            get => JsonSerializer.Serialize(GameBoard);
            set => GameBoard = JsonSerializer.Deserialize<string[][]>(value);
        }
    }

}
