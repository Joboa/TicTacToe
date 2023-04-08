﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TicTacToe.Data;

#nullable disable

namespace TicTacToe.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230408212125_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TicTacToe.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CurrentPlayer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameBoardJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("GameBoard");

                    b.Property<int>("Player1Id")
                        .HasColumnType("int");

                    b.Property<int>("Player2Id")
                        .HasColumnType("int");

                    b.Property<string>("Winner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("TicTacToe.Models.Move", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("BoardRow")
                        .HasColumnType("int");

                    b.Property<int>("BoardColumn")
                        .HasColumnType("int");

                    b.HasKey("GameId", "BoardRow", "BoardColumn");

                    b.ToTable("Moves");
                });

            modelBuilder.Entity("TicTacToe.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("TicTacToe.Models.PlayerMove", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BoardColumn")
                        .HasColumnType("int");

                    b.Property<int>("BoardRow")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<DateTime>("MoveTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PlayerMoves");
                });

            modelBuilder.Entity("TicTacToe.Models.Game", b =>
                {
                    b.HasOne("TicTacToe.Models.Player", "Player1")
                        .WithMany("GamesAsPlayer1")
                        .HasForeignKey("Player1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TicTacToe.Models.Player", "Player2")
                        .WithMany("GamesAsPlayer2")
                        .HasForeignKey("Player2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Player1");

                    b.Navigation("Player2");
                });

            modelBuilder.Entity("TicTacToe.Models.Player", b =>
                {
                    b.Navigation("GamesAsPlayer1");

                    b.Navigation("GamesAsPlayer2");
                });
#pragma warning restore 612, 618
        }
    }
}
