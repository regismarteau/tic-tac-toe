using Domain.Events;

namespace Domain;

public class TicTacToe
{
    private Grid grid = Grid.Init();
    public static TicTacToe StartNewGame()
    {
        return new();
    }

    public DomainEvents Mark(Player player, Cell cell)
    {
        this.grid = this.grid.Mark(new(player, cell));
        var events = DomainEvents.Raise(new CellMarked(player, cell));
        if (this.grid.Winner != Winner.NoOne)
        {
            return events.Add(new GameWon(this.grid.Winner == Winner.PlayerX ? Player.X : Player.O));
        }

        if (this.grid.IsCompleted)
        {
            return events.Add(new Draw());
        }

        return events;
    }
}
