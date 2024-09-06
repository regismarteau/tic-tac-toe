

namespace Domain;

public interface IDomainEvent;

public record CellMarked(Player Player, Cell Cell) : IDomainEvent;

public class TicTacToe
{
    private readonly IList<CellMarked> cellsMarked = [];
    public static TicTacToe StartNewGame()
    {
        return new();
    }

    public IDomainEvent Mark(Player player, Cell cell)
    {
        var lastCellMarked = this.cellsMarked.LastOrDefault();
        if ((lastCellMarked?.Player ?? Player.O) == player)
        {
            throw new BadPlayerException();
        }

        if(cellsMarked.Any(cellMarked => cellMarked.Cell == cell))
        {
            throw new CellAlreadyMarkedException();
        }

        var cellMarked = new CellMarked(player, cell);
        this.cellsMarked.Add(cellMarked);
        return cellMarked;
    }
}

public class BadPlayerException : Exception
{

}

public class CellAlreadyMarkedException : Exception
{

}

public enum Player
{
    X,
    O
}

public enum Cell
{
    TopLeft,
    TopMiddle,
    TopRight,
    Left,
    Middle,
    Right,
    BottomLeft,
    BottomMiddle,
    BottomRight
}