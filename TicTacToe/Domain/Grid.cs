using Domain.Exceptions;

namespace Domain;

internal class Grid
{
    private readonly IReadOnlyCollection<Mark> marks;

    private Grid(IReadOnlyCollection<Mark> marks)
    {
        this.marks = marks;
        this.Winner = this.EvaluateWinner();
    }

    public Winner Winner { get; }

    private Winner EvaluateWinner()
    {
        if (this.marks.Count == 0)
        {
            return Winner.NoOne;
        }

        var marksByPlayer = this.marks.GroupBy(mark => mark.Player).ToList();
        var playerXMarks = marksByPlayer.Single(player => player.Key == Player.X).Select(mark => mark.Cell).ToList();
        if (playerXMarks.ContainALine())
        {
            return Winner.PlayerX;
        }

        var playerOMarks = marksByPlayer.SingleOrDefault(player => player.Key == Player.O)?.Select(mark => mark.Cell).ToList() ?? [];
        if (playerOMarks.ContainALine())
        {
            return Winner.PlayerO;
        }

        return Winner.NoOne;
    }

    public static Grid Init()
    {
        return new([]);
    }

    public Grid Mark(Mark mark)
    {
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

public record Mark(Player Player, Cell Cell);

public enum Winner
{
    NoOne,
    PlayerX,
    PlayerO
}
