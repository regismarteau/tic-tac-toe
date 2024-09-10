using Microsoft.EntityFrameworkCore;

namespace Database;

public class TicTacToeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MarkEntity> Marks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MarkEntity>().HasKey(entity => new { entity.GameId, entity.Cell });
    }
}
