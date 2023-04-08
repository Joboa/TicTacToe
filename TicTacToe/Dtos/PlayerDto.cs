using System.ComponentModel.DataAnnotations.Schema;
using TicTacToe.Models;

namespace TicTacToe.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
