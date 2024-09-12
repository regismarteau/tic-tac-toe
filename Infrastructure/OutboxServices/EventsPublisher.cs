using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.OutboxServices;

public class EventsPublisher(TicTacToeDbContext dbContext, IMediator mediator)
{
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await this.PublishFirstEvent(stoppingToken);
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
            await mediator.Publish(domainEvent, stoppingToken);
            dbContext.Outbox.Remove(eventEntity);
            await dbContext.SaveChangesAsync(stoppingToken);
        }
        catch
        { }
    }
}
