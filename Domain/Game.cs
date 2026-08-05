using Domain.DomainEvents;
using Domain.Gameplay;
using Domain.ValueObjects;

namespace Domain;

public class Game
{
    private Game(GameId id, TicTacToe ticTacToe)
    {
        Id = id;
        TicTacToe = ticTacToe;
    }

    public GameId Id { get; }
    public TicTacToe TicTacToe { get; }

    public static Game Rehydrate(GameId id, IReadOnlyCollection<Mark> marks)
    {
        return new(id, TicTacToe.From(marks));
    }

    public static GameStarted Start()
    {
        return new GameStarted(GameId.New());
    }

    public Events Play(Player player, Cell cell)
    {
        var newTicTacToe = TicTacToe.Play(new(player, cell));
        var events = Events.Raise(new CellMarked(Id, player, cell));

        return newTicTacToe.Result switch
        {
            WonBy by => events.Add(new GameWon(Id, by.Player)),
            Draw => events.Add(new GameResultedAsADraw(Id)),
            _ => events
        };
    }
}