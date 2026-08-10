using Database;
using Microsoft.EntityFrameworkCore;
using RMediator.Abstractions;

namespace Infrastructure.OutboxServices;

public class EventsPublisher(TicTacToeDbContext dbContext, DomainEventToPublishAwaiter awaiter, DbContextSaveChanges changes, IPublishDomainEvent publisher)
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
        catch
        { }
    }
}
