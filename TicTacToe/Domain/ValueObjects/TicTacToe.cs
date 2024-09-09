using Domain.Exceptions;
using System.Diagnostics;

namespace Domain.ValueObjects;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class TicTacToe
{
    private readonly IReadOnlyCollection<Cell> XPlayerCells;
    private readonly IReadOnlyCollection<Cell> OPlayerCells;
    private readonly bool isFull;

    private TicTacToe(IReadOnlyCollection<Mark> marks)
    {
        this.Marks = marks;

        var marksByPlayer = this.Marks.GroupBy(mark => mark.Player).ToList();
        this.XPlayerCells = marksByPlayer.SingleOrDefault(player => player.Key == Player.X)?.Select(mark => mark.Cell).ToList() ?? [];
        this.OPlayerCells = marksByPlayer.SingleOrDefault(player => player.Key == Player.O)?.Select(mark => mark.Cell).ToList() ?? [];
        this.AvailableCells = Enum.GetValues<Cell>().Except(this.XPlayerCells).Except(this.OPlayerCells).ToList();
        this.isFull = this.AvailableCells.Count == 0;
        this.Result = this.EvaluateResult();
    }

    // TO DELETE
    private string DebuggerDisplay => string.Join("\r\n\r\n", Enumerable.Range(0, this.Marks.Count).Select(count => new TicTacToe(this.Marks.Take(count + 1).ToArray()).State));
    private string State => string.Join("", Enum.GetValues<Cell>().Select((cell, index) => (this.Marks.SingleOrDefault(mark => mark.Cell == cell)?.Player.ToString() ?? "-") + ((index + 1) % 3 == 0 ? Environment.NewLine : "")));

    public IReadOnlyCollection<Mark> Marks { get; }

    public IReadOnlyCollection<Cell> AvailableCells { get; }

    public Result Result { get; }

    public static TicTacToe Init()
    {
        return new([]);
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

        return new NoWinnerYet();
    }

    public TicTacToe Mark(Mark mark)
    {
        if (this.Result is Completed)
        {
            throw new GameAlreadyCompletedException();
        }

        if (mark.Player != this.GetNextPlayer())
        {
            throw new BadPlayerException();
        }

        if (!this.AvailableCells.Contains(mark.Cell))
        {
            throw new CellAlreadyMarkedException();
        }

        return new([.. this.Marks, mark]);
    }

    public Player GetNextPlayer()
    {
        if (this.Marks.Count == 0)
        {
            return Player.X;
        }

        return this.XPlayerCells.Count == this.OPlayerCells.Count ? Player.X : Player.O;
    }
}
