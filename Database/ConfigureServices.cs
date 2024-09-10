using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
    {
        return services.AddDbContext<TicTacToeDbContext>(p => p.UseInMemoryDatabase("Inmemory"));
    }
}