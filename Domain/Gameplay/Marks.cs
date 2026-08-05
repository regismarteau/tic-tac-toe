using System.Collections;

namespace Domain.ValueObjects;

public record Marks : IEnumerable<Mark>
{
    private readonly IReadOnlyCollection<Mark> values;

    public Marks(IReadOnlyCollection<Mark> values)
    {
        this.values = values;
        XPlayerCells = this.values.Where(mark => mark.PlayedByX).Select(mark => mark.Cell).ToList();
        OPlayerCells = this.values.Where(mark => mark.PlayedByO).Select(mark => mark.Cell).ToList();
        PlayedCells = XPlayerCells.Union(OPlayerCells).ToList();
        if (IsInvalid())
        {
            throw new IncorrectMarksException();
        }
    }

    public IReadOnlyCollection<Cell> XPlayerCells { get; }
    public IReadOnlyCollection<Cell> OPlayerCells { get; }
    public IReadOnlyCollection<Cell> PlayedCells { get; }

    private bool IsInvalid()
    {
        var cellsMarkedMoreThanOnce = PlayedCells.Distinct().Count() != values.Count;
        var badPlayerMovesCount = XPlayerCells.Count != OPlayerCells.Count && XPlayerCells.Count != OPlayerCells.Count + 1;
        return cellsMarkedMoreThanOnce || badPlayerMovesCount;
    }

    IEnumerator<Mark> IEnumerable<Mark>.GetEnumerator()
    {
        return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return values.GetEnumerator();
    }
}
