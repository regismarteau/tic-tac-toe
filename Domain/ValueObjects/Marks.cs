using Domain.Exceptions;
using System.Collections;

namespace Domain.ValueObjects;

public record Marks : IEnumerable<Mark>
{
    private readonly IReadOnlyCollection<Mark> values;

    public Marks(IReadOnlyCollection<Mark> values)
    {
        this.values = values;
        this.XPlayerCells = this.values.Where(mark => mark.PlayedByX).Select(mark => mark.Cell).ToList();
        this.OPlayerCells = this.values.Where(mark => mark.PlayedByO).Select(mark => mark.Cell).ToList();
        this.PlayedCells = this.XPlayerCells.Union(this.OPlayerCells).ToList();
        if (this.IsInvalid())
        {
            throw new IncorrectMarksException();
        }
    }

    public IReadOnlyCollection<Cell> XPlayerCells { get; }
    public IReadOnlyCollection<Cell> OPlayerCells { get; }
    public IReadOnlyCollection<Cell> PlayedCells { get; }

    private bool IsInvalid()
    {
        var cellsMarkedMoreThanOnce = this.PlayedCells.Distinct().Count() != this.values.Count;
        var badPlayerMovesCount = this.XPlayerCells.Count != this.OPlayerCells.Count && this.XPlayerCells.Count != this.OPlayerCells.Count + 1;
        return cellsMarkedMoreThanOnce || badPlayerMovesCount;
    }

    IEnumerator<Mark> IEnumerable<Mark>.GetEnumerator()
    {
        return this.values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.values.GetEnumerator();
    }
}
