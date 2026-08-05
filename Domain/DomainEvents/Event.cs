using RMediator.Abstractions;
using System.Collections;

namespace Domain.DomainEvents;

public class Events : IEnumerable<IDomainEvent>
{
    private readonly IReadOnlyCollection<IDomainEvent> events;

    private Events(IReadOnlyCollection<IDomainEvent> events)
    {
        this.events = events;
    }

    public static Events Raise(IDomainEvent @event)
    {
        return new([@event]);
    }

    public Events Add(IDomainEvent @event)
    {
        return new([.. events, @event]);
    }

    public IEnumerator<IDomainEvent> GetEnumerator()
    {
        return events.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return events.GetEnumerator();
    }
}
