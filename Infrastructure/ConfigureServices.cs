using Database;
using Microsoft.Extensions.DependencyInjection;
using Queries;
using UseCases;
using UseCases.Ports;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureTicTacToeServices(this IServiceCollection services)
    {
        return services
            .AddMediatR(config =>
                config
                .RegisterServicesFromAssemblies(
                    typeof(StartAGame).Assembly,
                    typeof(GetAllMarksFromGame).Assembly)
                .AddOpenBehavior(typeof(CommitOnCommandSucceed<,>), ServiceLifetime.Scoped))
            .AddScoped<IFindGame, GameRepository>()
            .AddScoped<IStoreGame, GameRepository>()
            .ConfigureDatabase();
    }
}
