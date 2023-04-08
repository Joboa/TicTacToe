namespace TicTacToe.Models
{
    public class PlayerMove
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public int BoardRow { get; set; }
        public int BoardColumn { get; set; }
        public DateTime MoveTime { get; set; }
    }
}
