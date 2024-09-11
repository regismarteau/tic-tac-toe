using Database;
using Domain.DomainEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Infrastructure.OutboxServices
{
    public class PublishEvents(IServiceProvider services) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scopedServices = services.CreateScope();
            var dbContext = scopedServices.ServiceProvider.GetRequiredService<TicTacToeDbContext>();
            var mediator = scopedServices.ServiceProvider.GetRequiredService<IMediator>();
            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishFirstEvent(dbContext, mediator, stoppingToken);
                await Task.Delay(10, stoppingToken);
            }
        }

        private static async Task PublishFirstEvent(TicTacToeDbContext dbContext, IMediator mediator, CancellationToken stoppingToken)
        {
            try
            {
                var @event = await dbContext.Outbox.FirstOrDefaultAsync(stoppingToken);
                if (@event is null)
                {
                    return;
                }
                dbContext.Outbox.Remove(@event);
                var domainEvent = JsonConvert.DeserializeObject<Event>(@event.Json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                if (domainEvent is not null)
                {
                    await mediator.Publish(domainEvent, stoppingToken);
                }
                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch { }
        }
    }
}
