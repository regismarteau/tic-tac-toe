using Domain;

namespace UseCases.Ports;

public interface IStoreGame
{
    Task Store(Game game);
}
public interface IFindGame
{
    Task<Game> Get();
}
