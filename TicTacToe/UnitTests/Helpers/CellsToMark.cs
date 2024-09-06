using Domain;
using FluentAssertions;

namespace UnitTests.Helpers;

public enum CellToMark
{
    Not, 
    X, 
    O
}

public static class CellsToMark
{
    public static IEnumerable<Mark> AsMarks(this CellToMark[] cellsToMark)
    {
        var turn = 0;
        IndexedCellToMark? nextPlayerXCell;
        IndexedCellToMark? nextPlayerOCell;
        var indexedCellsToMark = cellsToMark.Select((cellToMark, index) => new IndexedCellToMark(cellToMark, index)).ToArray();
        do
        {
            nextPlayerXCell = indexedCellsToMark.Where(mark => mark.Cell == CellToMark.X).Skip(turn).FirstOrDefault();
            if (nextPlayerXCell is not null)
            {
                yield return new(Player.X, nextPlayerXCell.Index.ToCell());
            }
            nextPlayerOCell = indexedCellsToMark.Where(mark => mark.Cell == CellToMark.O).Skip(turn).FirstOrDefault();
            if (nextPlayerOCell is not null)
            {
                yield return new(Player.O, nextPlayerOCell.Index.ToCell());
            }

            turn++;
        } while (nextPlayerXCell is not null && nextPlayerOCell is not null);

    }

    private static Cell ToCell(this int index)
    {
        return (Cell)index;
    }

    private record IndexedCellToMark(CellToMark Cell, int Index);
}