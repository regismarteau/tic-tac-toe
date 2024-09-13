using Domain;

namespace UseCases.Ports;

public interface IFindGame
{
    Task<Game> Get(GameId id);
}
