using Domain.Exceptions;
using Domain.Services;

namespace Domain.ValueObjects;

public record TicTacToe
{
    private readonly static IEnumerable<Cell> AllCells = Enum.GetValues<Cell>();
    private readonly IReadOnlyCollection<Cell> XPlayerCells;
    private readonly IReadOnlyCollection<Cell> OPlayerCells;
    private readonly bool isFull;

    private TicTacToe(IReadOnlyCollection<Mark> marks)
    {
        this.Marks = marks;
        this.XPlayerCells = this.Marks.Where(mark => mark.PlayedByX).Select(mark => mark.Cell).ToList();
        this.OPlayerCells = this.Marks.Where(mark => mark.PlayedByO).Select(mark => mark.Cell).ToList();
        this.AvailableCells = AllCells.Except(this.XPlayerCells).Except(this.OPlayerCells).ToList();
        this.isFull = this.AvailableCells.Count == 0;
        this.NextPlayer = this.XPlayerCells.Count == this.OPlayerCells.Count ? Player.X : Player.O;
        this.Result = this.EvaluateResult();
    }

    public IReadOnlyCollection<Mark> Marks { get; }
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
        if (this.XPlayerCells.ContainALine())
        {
            return new WonBy(Player.X);
        }

        if (this.OPlayerCells.ContainALine())
        {
            return new WonBy(Player.O);
        }

        if (this.isFull)
        {
            return new Draw();
        }

        return new Undetermined();
    }

    public TicTacToe Play(Mark mark)
    {
        if (this.Result is Completed)
        {
            throw new GameAlreadyCompletedException();
        }

        if (mark.Player != this.NextPlayer)
        {
            throw new BadPlayerException();
        }

        if (!this.AvailableCells.Contains(mark.Cell))
        {
            throw new CellAlreadyMarkedException();
        }

        return new([.. this.Marks, mark]);
    }
}
