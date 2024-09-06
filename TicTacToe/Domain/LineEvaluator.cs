namespace Domain;

internal static class LineEvaluator
{
    private readonly static IReadOnlyCollection<Line> HorizontalLines = [
        new Line([Cell.TopLeft, Cell.TopMiddle, Cell.TopRight]),
        new Line([Cell.Left, Cell.Middle, Cell.Right]),
        new Line([Cell.BottomLeft, Cell.BottomMiddle, Cell.BottomRight])
        ];

    private readonly static IReadOnlyCollection<Line> VerticalLines = [
        new Line([Cell.TopLeft, Cell.Left, Cell.BottomLeft]),
        new Line([Cell.TopMiddle, Cell.Middle, Cell.BottomMiddle]),
        new Line([Cell.TopRight, Cell.Right, Cell.BottomRight])
        ];

    private readonly static IReadOnlyCollection<Line> DiagonalLines = [
        new Line([Cell.TopLeft, Cell.Middle, Cell.BottomRight]),
        new Line([Cell.TopRight, Cell.Middle, Cell.BottomLeft])
        ];

    private readonly static IReadOnlyCollection<Line> Lines = [
        ..HorizontalLines,
        ..VerticalLines,
        ..DiagonalLines
        ];

    public static bool ContainALine(this IReadOnlyCollection<Cell> cells)
    {
        return Lines.Any(line => cells.Contains(line.Cells));
    }

    private static bool Contains<T>(this IEnumerable<T> left, IEnumerable<T> right)
    {
        return right.All(value => left.Contains(value));
    }

    private record Line(IReadOnlyCollection<Cell> Cells);
}