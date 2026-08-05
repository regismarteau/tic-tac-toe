using Database;
using Microsoft.EntityFrameworkCore;
using RMediator.Abstractions;

namespace Infrastructure.OutboxServices;

public class EventsPublisher(TicTacToeDbContext dbContext, IPublishDomainEvent publisher)
{
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PublishFirstEvent(stoppingToken);
            await Task.Delay(10, stoppingToken);
        }
    }

    private async Task PublishFirstEvent(CancellationToken stoppingToken)
    {
        try
        {
            var eventEntity = await dbContext.Outbox.FirstOrDefaultAsync(stoppingToken);
            if (eventEntity is null)
            {
                return;
            }
            var domainEvent = eventEntity.Deserialize();
            await publisher.Publish(domainEvent, stoppingToken);
            dbContext.Outbox.Remove(eventEntity);
            await dbContext.SaveChangesAsync(stoppingToken);
        }
        catch
        { }
    }
}
