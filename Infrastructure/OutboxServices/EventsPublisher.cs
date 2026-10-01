using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMediator.Abstractions;

namespace Infrastructure.OutboxServices;

public class EventsPublisher(TicTacToeDbContext dbContext, DomainEventToPublishAwaiter awaiter, DbContextSaveChanges changes, IPublishDomainEvent publisher, ILogger<EventsPublisher> logger)
{
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PublishFirstEvent(stoppingToken);
        }
    }

    private async Task PublishFirstEvent(CancellationToken stoppingToken)
    {
        try
        {
            await awaiter.WaitForADomainEvent(stoppingToken);
            var eventEntity = await dbContext.Outbox.FirstOrDefaultAsync(stoppingToken);
            if (eventEntity is null)
            {
                return;
            }
            var domainEvent = eventEntity.Deserialize();
            await publisher.Publish(domainEvent, stoppingToken);
            dbContext.Outbox.Remove(eventEntity);
            await changes.SaveAsync(stoppingToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogError(exception, "Publication of an outbox event failed");
        }
    }
}
