using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TicTacToe.Models;

namespace TicTacToe.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerMove> PlayerMoves { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
               .HasOne(g => g.Player1)
               .WithMany(p => p.GamesAsPlayer1)
               .HasForeignKey(g => g.Player1Id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
               .HasOne(g => g.Player2)
               .WithMany(p => p.GamesAsPlayer2)
               .HasForeignKey(g => g.Player2Id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
               .Ignore(g => g.GameBoard)
               .Property(g => g.GameBoardJson)
               .HasColumnName("GameBoard");

            modelBuilder.Entity<Player>()
               .HasMany(p => p.GamesAsPlayer1)
               .WithOne(g => g.Player1)
               .HasForeignKey(g => g.Player1Id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
               .HasMany(p => p.GamesAsPlayer2)
               .WithOne(g => g.Player2)
               .HasForeignKey(g => g.Player2Id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Move>()
               .HasKey(m => new { m.GameId, m.BoardRow, m.BoardColumn });
        }
    }
}
