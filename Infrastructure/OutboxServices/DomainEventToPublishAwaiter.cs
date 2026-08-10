namespace Infrastructure.OutboxServices;

public class DomainEventToPublishAwaiter
{
    private readonly SemaphoreSlim semaphore = new(0);
    public async Task WaitForADomainEvent(CancellationToken cancellationToken)
    {
        await semaphore.WaitAsync(cancellationToken);
    }

    public void NotifyForDomainEventsToPublish(int count)
    {
        semaphore.Release(count);
    }
}