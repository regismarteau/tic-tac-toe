using Microsoft.Extensions.DependencyInjection;
using UseCases;
using UseCases.Ports;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureTicTacToeServices(this IServiceCollection services)
    {
        return services
            .AddMediatR(config => config.RegisterServicesFromAssembly(typeof(StartAGame).Assembly))
            .AddSingleton<GameRepository>()
            .AddScoped<IFindGame>(p => p.GetRequiredService<GameRepository>())
            .AddScoped<IStoreGame>(p => p.GetRequiredService<GameRepository>());
    }
}
