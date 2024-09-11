using Microsoft.EntityFrameworkCore;

namespace Database;

public class TicTacToeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MarkEntity> Marks { get; set; }
    public DbSet<OutboxEntity> Outbox { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MarkEntity>().HasKey(entity => new { entity.GameId, entity.Cell });
        modelBuilder.Entity<OutboxEntity>().HasKey(entity => new { entity.EventId });
    }
}

public class OutboxEntity
{
    public Guid EventId { get; set; }
    public string Json { get; set; } = default!;
}
