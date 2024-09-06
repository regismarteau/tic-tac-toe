using Domain.Exceptions;

namespace Domain.ValueObjects;

internal class TicTacToe
{
    private const int TotalCellsCount = 9;
    private readonly IReadOnlyCollection<Mark> marks;

    private TicTacToe(IReadOnlyCollection<Mark> marks)
    {
        this.marks = marks;
        this.Result = this.EvaluateResult();
    }

    public Result Result { get; }
    private bool AllCellsAreMarked => this.marks.Count == TotalCellsCount;

    public static TicTacToe Init()
    {
        return new([]);
    }

    private Result EvaluateResult()
    {
        if (this.marks.Count == 0)
        {
            return new NoWinnerYet();
        }

        var marksByPlayer = this.marks.GroupBy(mark => mark.Player).ToList();
        var playerXMarks = marksByPlayer.Single(player => player.Key == Player.X).Select(mark => mark.Cell).ToList();
        if (playerXMarks.ContainALine())
        {
            return new WonBy(Player.X);
        }

        var playerOMarks = marksByPlayer.SingleOrDefault(player => player.Key == Player.O)?.Select(mark => mark.Cell).ToList() ?? [];
        if (playerOMarks.ContainALine())
        {
            return new WonBy(Player.O);
        }

        if (this.AllCellsAreMarked)
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

        if (this.marks.Any(cellMarked => cellMarked.Cell == mark.Cell))
        {
            throw new CellAlreadyMarkedException();
        }

        return new([.. this.marks, mark]);
    }

    private Player GetNextPlayer()
    {
        if (this.marks.Count == 0)
        {
            return Player.X;
        }

        var marksByPlayer = this.marks.GroupBy(mark => mark.Player).ToList();
        var playerXMarksCount = marksByPlayer.Single(player => player.Key == Player.X).Count();
        var playerOMarksCount = marksByPlayer.SingleOrDefault(player => player.Key == Player.O)?.Count() ?? 0;
        return playerXMarksCount == playerOMarksCount ? Player.X : Player.O;
    }
}
