using Domain.DomainEvents;
using Domain.ValueObjects;

namespace Domain;

public class Game
{

    private Game(GameId id, TicTacToe ticTacToe)
    {
        this.Id = id;
        this.TicTacToe = ticTacToe;
    }

    public GameId Id { get; }
    public TicTacToe TicTacToe { get; private set; }

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
        this.TicTacToe = this.TicTacToe.Mark(new(player, cell));
        var events = Events.Raise(new CellMarked(this.Id, player, cell));

        return this.TicTacToe.Result switch
        {
            WonBy by => events.Add(new GameWon(this.Id, by.Player)),
            Draw => events.Add(new GameResultedAsADraw(this.Id)),
            _ => events
        };
    }
}