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
            .AddMediatR(config =>
                config
                .RegisterServicesFromAssemblies(
                    typeof(StartAGame).Assembly,
                    typeof(GetGameState).Assembly)
                .AddOpenBehavior(typeof(CommitOnCommandSucceed<,>), ServiceLifetime.Scoped));
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
