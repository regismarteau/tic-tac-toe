using Domain.DomainEvents;

namespace UseCases.Ports;

public interface IStoreGame
{
    Task Store(Events events);
}
