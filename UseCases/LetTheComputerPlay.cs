using Domain;
using Domain.ValueObjects;
using UseCases.Ports;

namespace UseCases;

public record LetTheComputerPlay(GameId Id) : ICommand;

public class LetTheComputerPlayCommandHandler(IFindGame finder, IStoreGame storage) : CommandHandler<LetTheComputerPlay>
{
    protected override async Task Handle(LetTheComputerPlay command)
    {
        var game = await finder.Get(command.Id);
        var cellToMark = new UnbeatableMoveFinder(game.TicTacToe).FindBestCellToMark();
        var events = game.Play(Player.O, cellToMark);
        await storage.Store(events);
    }
}
