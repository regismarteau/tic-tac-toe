using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class TicTacToeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<MarkEntity> Marks { get; set; }
    public DbSet<OutboxEventEntity> Outbox { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<GameEntity>()
            .HasKey(entity => entity.Id);

        modelBuilder
            .Entity<MarkEntity>()
            .HasKey(entity => new { entity.GameId, entity.Cell });

        modelBuilder
            .Entity<MarkEntity>()
            .HasOne(entity => entity.Game)
            .WithMany(entity => entity.Marks)
            .HasForeignKey(entity => entity.GameId);

        modelBuilder
            .Entity<OutboxEventEntity>()
            .HasKey(entity => new { entity.EventId });
    }
}
