using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Helpers;

public enum DisplayedCell
{
    _, 
    X, 
    O
}

public static class DisplayedCellExtensions
{
    public static IEnumerable<Play> AsPlays(this DisplayedCell[] cells)
    {
        var turn = 0;
        IndexedCellToMark? nextPlayerXCell;
        IndexedCellToMark? nextPlayerOCell;
        var indexedCellsToMark = cells.Select((cellToMark, index) => new IndexedCellToMark(cellToMark, index)).ToArray();
        do
        {
            nextPlayerXCell = indexedCellsToMark.Where(mark => mark.Cell == DisplayedCell.X).Skip(turn).FirstOrDefault();
            if (nextPlayerXCell is not null)
            {
                yield return new(Player.X, nextPlayerXCell.Index.ToCell());
            }
            nextPlayerOCell = indexedCellsToMark.Where(mark => mark.Cell == DisplayedCell.O).Skip(turn).FirstOrDefault();
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

    private record IndexedCellToMark(DisplayedCell Cell, int Index);
}

public record Play(Player Player, Cell Cell);