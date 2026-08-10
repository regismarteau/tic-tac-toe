using Database;
using Database.Entities;
using Infrastructure.OutboxServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DbContextSaveChanges(TicTacToeDbContext dbContext, DomainEventToPublishAwaiter awaiter)
{
    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        var domainEventsInsertedCount = dbContext.ChangeTracker.Entries<OutboxEventEntity>().Count(e => e.State == EntityState.Added);
        await dbContext.SaveChangesAsync(cancellationToken);

        if (domainEventsInsertedCount > 0)
        {
            awaiter.NotifyForDomainEventsToPublish(domainEventsInsertedCount);
        }
    }
}