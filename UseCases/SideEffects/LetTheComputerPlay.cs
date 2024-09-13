using Domain.DomainEvents;
using Domain.Gameplay;
using Domain.UnbeatableComputer;
using UseCases.Ports;

namespace UseCases.SideEffects;

public class LetTheComputerPlayCommandHandler(IFindGame finder, IStoreGame storage) : DomainEventHandler<CellMarked>
{
    protected override async Task Handle(CellMarked @event)
    {
        if (@event.PlayedByComputer())
        {
            return;
        }
        var game = await finder.Get(@event.GameId);
        if (game.TicTacToe.Result is Completed)
        {
            return;
        }

        var cellToMark = new UnbeatablePlayFinder(game.TicTacToe).FindBestCellToMark();
        var events = game.Play(UserVersusComputer.Computer, cellToMark);
        await storage.Store(events);
    }
}
