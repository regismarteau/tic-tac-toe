using System.Collections;

namespace Domain.Events;

public interface IDomainEvent;

public class DomainEvents : IEnumerable<IDomainEvent>
{
    private readonly IReadOnlyCollection<IDomainEvent> events;

    private DomainEvents(IReadOnlyCollection<IDomainEvent> events)
    {
        this.events = events;
    }

    public static DomainEvents Raise(IDomainEvent @event)
    {
        return new([@event]);
    }

    public DomainEvents Add(IDomainEvent @event)
    {
        return new([.. this.events, @event]);
    }

    public IEnumerator<IDomainEvent> GetEnumerator()
    {
        return this.events.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.events.GetEnumerator();
    }
}
