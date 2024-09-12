using Domain.Services;
using Domain.ValueObjects;
using UseCases.Ports;

namespace UseCases.Commands;

public record Play(GameId Id, Cell Cell) : ICommand;

public class PlayCommandHandler(IFindGame finder, IStoreGame store) : CommandHandler<Play>
{
    protected override async Task Handle(Play command)
    {
        var game = await finder.Get(command.Id);
        var events = game.Play(UserVersusComputer.User, command.Cell);
        await store.Store(events);
    }
}
