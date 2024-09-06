using Domain;
using FluentAssertions;

namespace UnitTests;

public class TicTacToeMarkTests
{
    private readonly TicTacToe ticTacToe;

    public TicTacToeMarkTests()
    {
        this.ticTacToe = TicTacToe.StartNewGame();
    }


    [Fact]
    public void ThePlayerXCanMarkACellFirst()
    {
        var cellMarked = this.ticTacToe.Mark(Player.X, Cell.TopLeft);
        cellMarked.Should().Be(new CellMarked(Player.X, Cell.TopLeft));
    }

    [Fact]
    public void ThePlayerOCantMarkFirst()
    {
        this.Invoking(self => this.ticTacToe.Mark(Player.O, Cell.TopLeft))
            .Should()
            .ThrowExactly<BadPlayerException>();
    }

    [Fact]
    public void ThePlayerXCantMarkTwice()
    {
        this.ticTacToe.Mark(Player.X, Cell.TopLeft);
        this.Invoking(self => this.ticTacToe.Mark(Player.X, Cell.TopMiddle))
            .Should()
            .ThrowExactly<BadPlayerException>();
    }

    [Fact]
    public void ThePlayerOCanMarkAfterPlayerX()
    {
        this.ticTacToe.Mark(Player.X, Cell.TopLeft);
        var cellMarked = this.ticTacToe.Mark(Player.O, Cell.Right);
        cellMarked.Should().Be(new CellMarked(Player.O, Cell.Right));
    }

    [Fact]
    public void APlayerCantMarkACellAlreadyMarked()
    {
        this.ticTacToe.Mark(Player.X, Cell.TopLeft);
        this.ticTacToe.Mark(Player.O, Cell.Middle);
        this.ticTacToe.Mark(Player.X, Cell.BottomRight);

        this.Invoking(self => this.ticTacToe.Mark(Player.O, Cell.TopLeft))
            .Should()
            .ThrowExactly<CellAlreadyMarkedException>();
    }
}