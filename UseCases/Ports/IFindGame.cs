using Domain;
using Domain.ValueObjects;

namespace UseCases.Ports;

public interface IFindGame
{
    Task<Game> Get(GameId id);
}
