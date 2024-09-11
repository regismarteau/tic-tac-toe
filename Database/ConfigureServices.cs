using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
    {
        string databaseName = Guid.NewGuid().ToString();
        return services.AddDbContext<TicTacToeDbContext>(p => p.UseInMemoryDatabase(databaseName));
    }
}