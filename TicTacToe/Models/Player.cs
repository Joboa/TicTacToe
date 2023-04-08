using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [InverseProperty("Player1")]
        public List<Game> GamesAsPlayer1 { get; set; }
        [InverseProperty("Player2")]
        public List<Game> GamesAsPlayer2 { get; set; }
    }
}
