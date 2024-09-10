using Domain;
using UseCases.Ports;

namespace UseCases;

public record StartAGame : ICommand<Guid>;

public class StartAGameCommandHandler(IStoreGame store) : CommandHandler<StartAGame, Guid>
{
    protected override async Task<Guid> Handle(StartAGame command)
    {
        var gameStarted = Game.Start();
        await store.Store(gameStarted);
        return gameStarted.Id.Value;
    }
}
