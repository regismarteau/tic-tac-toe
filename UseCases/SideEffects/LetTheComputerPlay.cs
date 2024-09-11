using Domain;
using Domain.DomainEvents;
using Domain.ValueObjects;
using UseCases.Ports;

namespace UseCases.SideEffects;

public class LetTheComputerPlayCommandHandler(IFindGame finder, IStoreGame storage) : DomainEventHandler<CellMarked>
{
    protected override async Task Handle(CellMarked @event)
    {
        if (@event.Player == Player.O)
        {
            return;
        }
        var game = await finder.Get(@event.GameId);
        if (game.TicTacToe.Result is Completed)
        {
            return;
        }

        var cellToMark = new UnbeatablePlayFinder(game.TicTacToe).FindBestCellToMark();
        var events = game.Play(Player.O, cellToMark);
        await storage.Store(events);
    }
}
