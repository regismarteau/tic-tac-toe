using Domain;
using UseCases.Ports;

namespace Infrastructure;

public class GameRepository : IFindGame, IStoreGame
{
    private Game? game;

    public Task<Game> Get()
    {
        return Task.FromResult(this.game ?? throw new InvalidOperationException("Unable to find the game"));
    }

    public Task Store(Game game)
    {
        this.game = game;
        return Task.CompletedTask;
    }
}
