using MediatR;

namespace UseCases.SideEffects;

public abstract class DomainEventHandler<TNotification> : INotificationHandler<TNotification>
    where TNotification : INotification
{
    protected abstract Task Handle(TNotification @event);
    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        return this.Handle(notification);
    }
}
