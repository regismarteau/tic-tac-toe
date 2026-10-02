namespace Domain.Gameplay;

internal static class LineEvaluator
{
    private static readonly IReadOnlyCollection<Line> HorizontalLines =
    [
        new([Cell.TopLeft, Cell.TopMiddle, Cell.TopRight]),
        new([Cell.Left, Cell.Middle, Cell.Right]),
        new([Cell.BottomLeft, Cell.BottomMiddle, Cell.BottomRight])
    ];

    private static readonly IReadOnlyCollection<Line> VerticalLines =
    [
        new([Cell.TopLeft, Cell.Left, Cell.BottomLeft]),
        new([Cell.TopMiddle, Cell.Middle, Cell.BottomMiddle]),
        new([Cell.TopRight, Cell.Right, Cell.BottomRight])
    ];

    private static readonly IReadOnlyCollection<Line> DiagonalLines =
    [
        new([Cell.TopLeft, Cell.Middle, Cell.BottomRight]),
        new([Cell.TopRight, Cell.Middle, Cell.BottomLeft])
    ];

    private static readonly IReadOnlyCollection<Line> Lines =
    [
        .. HorizontalLines,
        .. VerticalLines,
        .. DiagonalLines
    ];

    public static bool ContainALine(this IReadOnlyCollection<Cell> cells) => Lines.Any(line => cells.Contains(line.Cells));

    private static bool Contains<T>(this IEnumerable<T> left, IEnumerable<T> right) => right.All(value => left.Contains(value));

    private record Line(IReadOnlyCollection<Cell> Cells);
}
