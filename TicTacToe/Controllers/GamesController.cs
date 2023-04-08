using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Dtos;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public GamesController(DataContext dbContext) 
        {  
            _dbContext = dbContext;
        }

        // Start game
        [HttpPost]
        public ActionResult<GameDto> StartGame([FromBody] GameDto gameDto)
        {
            if (string.IsNullOrEmpty(gameDto.Player1?.Name) || string.IsNullOrEmpty(gameDto.Player2?.Name))
            {
                return BadRequest("Both players must have a name.");
            }

            try
            {   
                var player1 = new Player { Name = gameDto.Player1.Name };
                var player2 = new Player { Name = gameDto.Player2.Name };

                _dbContext.Players.AddRange(player1, player2);
                _dbContext.SaveChanges();

                var game = new Game()
                {
                    Player1Id = player1.Id,
                    Player1 = player1,
                    Player2Id = player2.Id,
                    Player2 = player2,
                    CurrentPlayer = "X",
                    GameBoard = new string[][]
                    {
                        new string[] { "-", "-", "-" },
                        new string[] { "-", "-", "-" },
                        new string[] { "-", "-", "-" }
                    }
                };

                _dbContext.Games.Add(game);
                _dbContext.SaveChanges();

                var gameResultsDto = new GameDto()
                {
                    Id = game.Id,
                    Player1 = new PlayerDto() { Id = player1.Id, Name = player1.Name },
                    Player2 = new PlayerDto() { Id = player2.Id, Name = player2.Name }
                };

                return CreatedAtAction(nameof(StartGame), new { id = gameResultsDto.Id }, gameResultsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating the game. Details: {ex?.Message}");
            }
        }
    }

}
