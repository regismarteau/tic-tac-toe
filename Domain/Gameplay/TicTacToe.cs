using Domain.Gameplay;
using Domain.ValueObjects;

namespace Domain.ValueObjects;

public record TicTacToe
{
    private readonly static IEnumerable<Cell> AllCells = Enum.GetValues<Cell>();
    private readonly bool isFull;

    private TicTacToe(IReadOnlyCollection<Mark> marks)
    {
        Marks = new(marks);
        AvailableCells = AllCells.Except(Marks.PlayedCells).ToList();
        isFull = AvailableCells.Count == 0;
        NextPlayer = Marks.XPlayerCells.Count == Marks.OPlayerCells.Count ? Player.X : Player.O;
        Result = EvaluateResult();
    }

    public Marks Marks { get; }
    public IReadOnlyCollection<Cell> AvailableCells { get; }
    public Result Result { get; }
    public Player NextPlayer { get; }

    public static TicTacToe New()
    {
        return new([]);
    }

    internal static TicTacToe From(IReadOnlyCollection<Mark> marks)
    {
        return new TicTacToe(marks);
    }

    private Result EvaluateResult()
    {
        if (Marks.XPlayerCells.ContainALine())
        {
            return new WonBy(Player.X);
        }

        if (Marks.OPlayerCells.ContainALine())
        {
            return new WonBy(Player.O);
        }

        if (isFull)
        {
            return new Draw();
        }

        return new Undetermined();
    }

    public TicTacToe Play(Mark mark)
    {
        if (Result is Completed)
        {
            throw new GameAlreadyCompletedException();
        }

        if (mark.Player != NextPlayer)
        {
            throw new BadPlayerException();
        }

        if (!AvailableCells.Contains(mark.Cell))
        {
            throw new CellAlreadyMarkedException();
        }

        return new([.. Marks, mark]);
    }
}
