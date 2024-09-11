using Database;
using Infrastructure.OutboxServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Queries;
using UseCases.Commands;
using UseCases.Ports;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureTicTacToeServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMediatR(config =>
                config
                .RegisterServicesFromAssemblies(
                    typeof(StartAGame).Assembly,
                    typeof(GetGameState).Assembly)
                .AddOpenBehavior(typeof(CommitOnCommandSucceed<,>), ServiceLifetime.Scoped))
            .AddScoped<IFindGame, GameRepository>()
            .AddScoped<IStoreGame, GameRepository>()
            .AddHostedService<EventsPublisher>()
            .ConfigureDatabase(configuration);
    }
}
