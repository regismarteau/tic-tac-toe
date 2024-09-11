using Domain;
using Domain.DomainEvents;
using Domain.ValueObjects;
using UseCases.Ports;

namespace UseCases.SideEffects;

public class LetTheComputerPlayCommandHandler(IFindGame finder, IStoreGame storage) : DomainEventHandler<CellMarked>
{
    protected override async Task Handle(CellMarked @event)
    {
        var game = await finder.Get(@event.GameId);
        var cellToMark = new UnbeatableMoveFinder(game.TicTacToe).FindBestCellToMark();
        var events = game.Play(Player.O, cellToMark);
        await storage.Store(events);
    }
}
