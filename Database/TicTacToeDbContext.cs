using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options) : DbContext(options)
{
    public DbSet<GameEntity> Games => Set<GameEntity>();
    public DbSet<MarkEntity> Marks => Set<MarkEntity>();
    public DbSet<OutboxEventEntity> Outbox => Set<OutboxEventEntity>();

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
