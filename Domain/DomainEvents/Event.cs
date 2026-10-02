using System.Collections;
using RMediator.Abstractions;

namespace Domain.DomainEvents;

public class Events : IEnumerable<IDomainEvent>
{
    private readonly IReadOnlyCollection<IDomainEvent> events;

    private Events(IReadOnlyCollection<IDomainEvent> events)
    {
        this.events = events;
    }

    public IEnumerator<IDomainEvent> GetEnumerator() => events.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => events.GetEnumerator();

    public static Events Raise(IDomainEvent @event) => new([@event]);

    public Events Add(IDomainEvent @event) => new([.. events, @event]);
}
