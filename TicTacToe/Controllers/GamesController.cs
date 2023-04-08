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

         // Register player moves
        [HttpPost("{id}/players/{playerId}/moves")]
        public ActionResult RegisterPlayerMove(int id, int playerId, [FromBody] MoveDto moveDto)
        {
            // Find the player
            var player = _dbContext.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                return NotFound("Player not found");
            }

            // Find game
            var game = _dbContext.Games
                            .Include(g => g.Player1)
                            .Include(g => g.Player2)
                            .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            // Check if the player is one of the players in the game
            if (player != game.Player1 && player != game.Player2)
            {
                return BadRequest("Player is not part of this game");
            }

            // Game variables
            var currentPlayer = game.CurrentPlayer;
            var player1Id = game.Player1Id;
            var player2Id = game.Player2Id;
            var gameBoard = game.GameBoard;

            // Check if the game has ended
            if (game.Winner != "")
            {
                return BadRequest("Game has already ended");
            }

            // Check if it is a player's turn
            if (!IsPlayerTurn(game, playerId))
            {
                return BadRequest($"It is {game.CurrentPlayer}'s turn");
            }

            // Check if gameboard position is occupied
            if (gameBoard[moveDto.BoardRow][moveDto.BoardColumn] != "-")
            {
                return BadRequest("This position is already occupied");
            }

            // Update the game board with the player's move
            gameBoard[moveDto.BoardRow][moveDto.BoardColumn] = currentPlayer;

            // Check if the player's move has won the game
            if (CheckForWinner(gameBoard, currentPlayer))
            {
                game.Winner = currentPlayer;
                RegisterMoves(game.Id, playerId, moveDto.BoardRow, moveDto.BoardColumn);
                return Ok($"Move registered successfully. {currentPlayer} wins!");
            }
            else if (CheckForDraw(gameBoard))
            {
                game.Winner = "Draw";
                RegisterMoves(game.Id, playerId, moveDto.BoardRow, moveDto.BoardColumn);
                return Ok("Draw! No one wins");
            }
            else
            {
                // Switch the current player
                game.CurrentPlayer = currentPlayer == "X" ? "O" : "X";
            }

            // Register the moves and associate it with the game and player
            RegisterMoves(game.Id, playerId, moveDto.BoardRow, moveDto.BoardColumn);
           
            return Ok("Move registered successfully");
        }

         // Register player moves
        [HttpPost("{id}/player/{playerId}/moves")]
        public ActionResult RecordPlayerMove(int id, int playerId, [FromBody] MoveDto moveDto)
        {
            // Find the player
            var player = _dbContext.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                return NotFound("Player not found");
            }

            // Find game
            var game = _dbContext.Games
                            .Include(g => g.Player1)
                            .Include(g => g.Player2)
                            .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            // Check if the player is one of the players in the game
            if (player != game.Player1 && player != game.Player2)
            {
                return BadRequest("Player is not part of this game");
            }

            // Game variables
            var currentPlayer = game.CurrentPlayer;
            var player1Id = game.Player1Id;
            var player2Id = game.Player2Id;
            var gameBoard = game.GameBoard;

            // Check if the game has ended
            if (game.Winner != "")
            {
                return BadRequest("Game has already ended");
            }

            // Check if it is a player's turn
            if (!IsPlayerTurn(game, playerId))
            {
                return BadRequest($"It is {game.CurrentPlayer}'s turn");
            }

            // Check if gameboard position is occupied
            if (gameBoard[moveDto.BoardRow][moveDto.BoardColumn] != "-")
            {
                return BadRequest("This position is already occupied");
            }

            // Update the game board with the player's move
            gameBoard[moveDto.BoardRow][moveDto.BoardColumn] = currentPlayer;

            // Check if the player's move has won the game
            if (CheckForWinner(gameBoard, currentPlayer))
            {
                game.Winner = currentPlayer;
                RegisterMoves(game.Id, playerId, moveDto.BoardRow, moveDto.BoardColumn);
                return Ok($"Move registered successfully. {currentPlayer} wins!");
            }
            else if (CheckForDraw(gameBoard))
            {
                game.Winner = "Draw";
                RegisterMoves(game.Id, playerId, moveDto.BoardRow, moveDto.BoardColumn);
                return Ok("Draw! No one wins");
            }
            else
            {
                // Switch the current player
                game.CurrentPlayer = currentPlayer == "X" ? "O" : "X";
            }

            // Register the moves and associate it with the game and player
            RegisterMoves(game.Id, playerId, moveDto.BoardRow, moveDto.BoardColumn);
           
            return Ok("Move registered successfully");
        }

        // Get all running games
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetRunningGames()
        {
            var gamesList = _dbContext.Games
                .Select(g => new
                {
                    GameId = g.Id,
                    Player1 = new
                    {
                        g.Player1.Name,
                        Moves = _dbContext.PlayerMoves
                                                .Count(pm => pm.GameId == g.Id && pm.PlayerId == g.Player1Id),
                        LastMove = _dbContext.PlayerMoves
                                                .OrderByDescending(pm => pm.MoveTime)
                                                .Where(pm => pm.GameId == g.Id && pm.PlayerId == g.Player1Id)
                                                .Select(pm => new { pm.BoardRow, pm.BoardColumn, pm.MoveTime })
                                                .FirstOrDefault()
                    },
                    Player2 = new
                    {
                        g.Player2.Name,
                        Moves = _dbContext.PlayerMoves
                                                .Count(pm => pm.GameId == g.Id && pm.PlayerId == g.Player2Id),
                        LastMove = _dbContext.PlayerMoves
                                                .OrderByDescending(pm => pm.MoveTime)
                                                .Where(pm => pm.GameId == g.Id && pm.PlayerId == g.Player2Id)
                                                .Select(pm => new { pm.BoardRow, pm.BoardColumn, pm.MoveTime })
                                                .FirstOrDefault()
                    },
                    NumMoves = _dbContext.PlayerMoves.Count(pm => pm.GameId == g.Id),
                    g.Winner,
                    Board = g.GameBoard
                })
                .ToList();


            return Ok(gamesList);
        }

        private static bool IsPlayerTurn(Game game, int playerId)
        {
            var currentPlayer = game.CurrentPlayer;
            var player1Id = game.Player1Id;
            var player2Id = game.Player2Id;

            // Check if it is the player's turn
            if (currentPlayer == "O" && player1Id == playerId)
            {
                return true;
            }
            else if (currentPlayer == "X" && player2Id == playerId)
            {
                return true;
            }

            return false;
        }

        private void RegisterMoves(int gameId, int playerId, int boardRow, int boardColumn)
        {
            // Register a new move and associate it with the game
            var move = new Move
            {
                GameId = gameId,
                BoardRow = boardRow,
                BoardColumn = boardColumn
            };

            _dbContext.Moves.Add(move);
            _dbContext.SaveChanges();

            // Track player moves
            var playerMove = new PlayerMove
            {
                PlayerId = playerId,
                GameId = gameId,
                BoardRow = boardRow,
                BoardColumn = boardColumn,
                MoveTime = DateTime.Now
            };

            _dbContext.PlayerMoves.Add(playerMove);
            _dbContext.SaveChanges();
        }

        private static bool CheckForWinner(string[][] gameBoard, string player)
        {
            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (gameBoard[i][0] == player && gameBoard[i][1] == player && gameBoard[i][2] == player)
                {
                    return true;
                }
            }

            // Check columns
            for (int j = 0; j < 3; j++)
            {
                if (gameBoard[0][j] == player && gameBoard[1][j] == player && gameBoard[2][j] == player)
                {
                    return true;
                }
            }

            // Check diagonals
            if (gameBoard[0][0] == player && gameBoard[1][1] == player && gameBoard[2][2] == player)
            {
                return true;
            }

            if (gameBoard[0][2] == player && gameBoard[1][1] == player && gameBoard[2][0] == player)
            {
                return true;
            }

            return false;
        }

        // Check for draw
        private static bool CheckForDraw(string[][] board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i][j] == "-")
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }

}
