using MediatR;
using System.Collections;

namespace Domain.DomainEvents;

public record Event : INotification;

public class Events : IEnumerable<Event>
{
    private readonly IReadOnlyCollection<Event> events;

    private Events(IReadOnlyCollection<Event> events)
    {
        this.events = events;
    }

    public static Events Raise(Event @event)
    {
        return new([@event]);
    }

    public Events Add(Event @event)
    {
        return new([.. this.events, @event]);
    }

    public IEnumerator<Event> GetEnumerator()
    {
        return this.events.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.events.GetEnumerator();
    }

    public static implicit operator Events(Event @event)
    {
        return new([@event]);
    }
}
