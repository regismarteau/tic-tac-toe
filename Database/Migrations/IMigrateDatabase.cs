using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Migrations
{
    public interface IMigrateDatabase
    {
        Task Migrate();
    }

    public class DatabaseMigration(IServiceProvider serviceProvider) : IMigrateDatabase
    {
        public async Task Migrate()
        {
            using var scope = serviceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<TicTacToeDbContext>().Database.MigrateAsync();
        }
    }
}
