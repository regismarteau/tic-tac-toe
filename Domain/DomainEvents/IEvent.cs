using System.Collections;

namespace Domain.DomainEvents;

public record IEvent;

public class Events : IEnumerable<IEvent>
{
    private readonly IReadOnlyCollection<IEvent> events;

    private Events(IReadOnlyCollection<IEvent> events)
    {
        this.events = events;
    }

    public static Events Raise(IEvent @event)
    {
        return new([@event]);
    }

    public Events Add(IEvent @event)
    {
        return new([.. this.events, @event]);
    }

    public IEnumerator<IEvent> GetEnumerator()
    {
        return this.events.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.events.GetEnumerator();
    }

    public static implicit operator Events(IEvent @event)
    {
        return new([@event]);
    }
}
