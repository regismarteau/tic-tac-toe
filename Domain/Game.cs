using Domain.Events;
using Domain.ValueObjects;

namespace Domain;

public class Game
{
    private TicTacToe ticTacToe;

    private Game(TicTacToe ticTacToe)
    {
        this.Id = GameId.New();
        this.ticTacToe = ticTacToe;
    }

    public GameId Id { get; }

    public static Game Start()
    {
        return new(TicTacToe.Init());
    }

    public DomainEvents Play(Player player, Cell cell)
    {
        this.ticTacToe = this.ticTacToe.Mark(new(player, cell));
        var events = DomainEvents.Raise(new CellMarked(player, cell));

        return this.ticTacToe.Result switch
        {
            WonBy by => events.Add(new GameWon(by.Player)),
            Draw => events.Add(new GameResultedAsADraw()),
            _ => events
        };
    }
}