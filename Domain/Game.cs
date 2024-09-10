using Domain.DomainEvents;
using Domain.ValueObjects;

namespace Domain;

public class Game
{
    private TicTacToe ticTacToe;

    private Game(GameId id, TicTacToe ticTacToe)
    {
        this.Id = id;
        this.ticTacToe = ticTacToe;
    }

    public GameId Id { get; }

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
        this.ticTacToe = this.ticTacToe.Mark(new(player, cell));
        var events = Events.Raise(new CellMarked(this.Id, player, cell));

        return this.ticTacToe.Result switch
        {
            WonBy by => events.Add(new GameWon(by.Player)),
            Draw => events.Add(new GameResultedAsADraw()),
            _ => events
        };
    }
}