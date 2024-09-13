using Database.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class ConfigureServices
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<TicTacToeDbContext>(p => p.UseSqlite(configuration.GetConnectionString("Database")))
            .AddSingleton<IMigrateDatabase, DatabaseMigration>();
    }
}