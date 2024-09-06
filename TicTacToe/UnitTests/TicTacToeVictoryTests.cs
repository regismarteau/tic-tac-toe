using Domain;
using Domain.Events;
using FluentAssertions;
using UnitTests.Helpers;

namespace UnitTests;

public class TicTacToeVictoryTests
{
    protected const CellToMark X = CellToMark.X;
    protected const CellToMark O = CellToMark.O;
    protected const CellToMark _ = CellToMark.Not;
    private readonly TicTacToe ticTacToe;

    public TicTacToeVictoryTests()
    {
        this.ticTacToe = TicTacToe.StartNewGame();
    }

    [Theory]
    [InlineData([
        X, X, X,
        O, O, _,
        _, _, _])]        
    [InlineData([
        O, O, _,
        X, X, X,
        _, _, _])]
    [InlineData([
        _, _, _,
        O, O, _,
        X, X, X])]
    [InlineData([
        X, O, _,
        X, O, _,
        X, _, _])]
    [InlineData([
        _, X, O,
        _, X, O,
        _, X, _])]
    [InlineData([
        O, _, X,
        O, _, X,
        _, _, X])]
    [InlineData([
        X, O, _,
        O, X, _,
        _, _, X])]
    [InlineData([
        _, O, X,
        O, X, _,
        X, _, _])]
    public void ThePlayerXWinsTheGameWhenALineIsDrawn(params CellToMark[] cellsToMark)
    {
        this.PlayAGame(cellsToMark).Should().Be(new GameWon(Player.X));
    }

    [Theory]
    [InlineData([
        X, _, X,
        O, O, O,
        _, X, _
        ])]
    [InlineData([
        O, _, X,
        X, O, O,
        X, X, O
        ])]
    public void ThePlayerOWinsTheGameWhenALineIsDrawn(params CellToMark[] cellsToMark)
    {
        this.PlayAGame(cellsToMark).Should().Be(new GameWon(Player.O));
    }

    [Theory]
    [InlineData([
        X, O, X,
        O, O, X,
        X, X, O
        ])]
    [InlineData([
        X, O, O,
        O, X, X,
        X, X, O
        ])]
    [InlineData([
        X, O, X,
        X, X, O,
        O, X, O
        ])]
    [InlineData([
        X, O, X,
        X, O, O,
        O, X, X
        ])]
    public void ItIsPossibleThatNoOneWins(params CellToMark[] cellsToMark)
    {
        this.PlayAGame(cellsToMark).Should().Be(new Draw());
    }

    private GameCompleted? PlayAGame(CellToMark[] cellsToMark)
    {
        foreach (var mark in cellsToMark.AsMarks())
        {
            var gameCompleted = this.ticTacToe.Mark(mark.Player, mark.Cell)
                .OfType<GameCompleted>()
                .FirstOrDefault();

            if (gameCompleted is not null)
            {
                return gameCompleted;
            }
        }

        return null;
    }
}
