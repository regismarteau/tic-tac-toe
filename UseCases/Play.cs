using Domain.ValueObjects;
using UseCases.Ports;

namespace UseCases;

public record Play(Cell Cell) : ICommand;

public class PlayCommandHandler(IFindGame finder, IStoreGame store) : CommandHandler<Play>
{
    protected override async Task Handle(Play command)
    {
        var game = await finder.Get();
        game.Play(Player.X, command.Cell);
        await store.Store(game);
    }
}
