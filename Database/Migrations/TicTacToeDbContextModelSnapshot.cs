﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(TicTacToeDbContext))]
    partial class TicTacToeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Database.Entities.GameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Result")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Database.Entities.MarkEntity", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Cell")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Player")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameId", "Cell");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("Database.Entities.OutboxEventEntity", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EventId");

                    b.ToTable("Outbox");
                });

            modelBuilder.Entity("Database.Entities.MarkEntity", b =>
                {
                    b.HasOne("Database.Entities.GameEntity", "Game")
                        .WithMany("Marks")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Database.Entities.GameEntity", b =>
                {
                    b.Navigation("Marks");
                });
#pragma warning restore 612, 618
        }
    }
}
