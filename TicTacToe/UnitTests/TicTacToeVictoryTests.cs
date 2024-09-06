using Domain;
using Domain.Events;
using FluentAssertions;
using System.Collections;

namespace UnitTests;

public enum CellToMark
{
    Not, X, O
}

public class TicTacToeVictoryTests
{
    private readonly TicTacToe ticTacToe;

    public TicTacToeVictoryTests()
    {
        this.ticTacToe = TicTacToe.StartNewGame();
    }

    [Theory]
    [ClassData(typeof(Play))]
    public void ThePlayerWinsTheGameWhenALineIsDrawn(params CellToMark[] cellsToMark)
    {
        GameWon? gameWon = null;
        foreach (var mark in cellsToMark.ToMarks())
        {
            gameWon ??= this.ticTacToe.Mark(mark.Player, mark.Cell)
                .OfType<GameWon>()
                .FirstOrDefault();
        }

        gameWon.Should().Be(new GameWon(Player.X));
    }
}

public class Play : IEnumerable<object[]>
{
    private const CellToMark X = CellToMark.X;
    private const CellToMark O = CellToMark.O;
    private const CellToMark _ = CellToMark.Not;

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            X, X, X,
            O, O, _,
            _, _, _];
        yield return [
            O, O, _,
            X, X, X,
            _, _, _];
        yield return [
            _, _, _,
            O, O, _,
            X, X, X];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

public static class CellsToMarkExtensions
{
    public static IEnumerable<Mark> ToMarks(this CellToMark[] cellsToMark)
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