using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.OutboxServices;

public class BackgroundEventsPublisherService(IServiceProvider services) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scopedServices = services.CreateScope();
        var publisher = scopedServices.ServiceProvider.GetRequiredService<EventsPublisher>();
        await publisher.ExecuteAsync(stoppingToken);
    }
}
