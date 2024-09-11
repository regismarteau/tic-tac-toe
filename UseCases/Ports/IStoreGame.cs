using Domain;
using Domain.DomainEvents;
using Domain.ValueObjects;

namespace UseCases.Ports;

public interface IStoreGame
{
    Task Store(Events events);
}
public interface IFindGame
{
    Task<Game> Get(GameId id);
}
