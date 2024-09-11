using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AcceptanceTests.Configuration
{
    public static class ConfigureServices
    {
        public static IServiceCollection SubstituteServices(this IServiceCollection services)
        {
            var databaseName = Guid.NewGuid().ToString();
            return services
                .AddScoped<AsynchronousSideEffectsAwaiter>()
                .AddScoped(_ => new DbContextOptionsBuilder<TicTacToeDbContext>()
                    .UseInMemoryDatabase(databaseName).Options);
        }
    }
}
