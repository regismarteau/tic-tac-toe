using Database;
using Infrastructure.OutboxServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Queries;
using RMediator.DependencyInjection;
using UseCases.Commands;
using UseCases.Ports;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddTicTacToeServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDispatcher()
            .AddRepositories()
            .AddEventsPublisher()
            .AddDatabase(configuration);
    }

    private static IServiceCollection AddDispatcher(this IServiceCollection services)
    {
        return services
            .AddMediator(config =>
                config.ScanAssemblies(
                    typeof(StartAGame).Assembly,
                    typeof(GetGameState).Assembly)
                .AddMiddlewares(typeof(CommitOnCommandSucceed<,>), typeof(CommitOnCommandSucceed<>)));
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IFindGame, GameRepository>()
            .AddScoped<IStoreGame, GameRepository>();
    }

    private static IServiceCollection AddEventsPublisher(this IServiceCollection services)
    {
        return services
            .AddHostedService<BackgroundEventsPublisherService>()
            .AddScoped<EventsPublisher>();
    }
}
