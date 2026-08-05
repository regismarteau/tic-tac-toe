using Domain;
using Domain.UnbeatableComputer;
using Domain.ValueObjects;
using RMediator.Abstractions;
using UseCases.Ports;

namespace UseCases.Commands;

public record Play(GameId Id, Cell Cell) : ICommand;

public class PlayCommandHandler(IFindGame finder, IStoreGame store) : IHandleCommand<Play>
{
    public async Task Handle(Play command, CancellationToken cancellationToken)
    {
        var game = await finder.Get(command.Id);
        var events = game.Play(UserVersusComputer.User, command.Cell);
        await store.Store(events);
    }
}
