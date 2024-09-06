using Domain;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests;

public class TicTacToeMarkTests
{
    private readonly Game game;

    public TicTacToeMarkTests()
    {
        this.game = Game.Start();
    }

    [Fact]
    public void ThePlayerXCanMarkACellFirst()
    {
        var cellMarked = this.game.Play(Player.X, Cell.TopLeft);
        cellMarked
            .Should()
            .BeEquivalentTo([new CellMarked(Player.X, Cell.TopLeft)]);
    }

    [Fact]
    public void ThePlayerOCantMarkFirst()
    {
        this.Invoking(self => this.game.Play(Player.O, Cell.TopLeft))
            .Should()
            .ThrowExactly<BadPlayerException>();
    }

    [Fact]
    public void ThePlayerXCantMarkTwice()
    {
        this.game.Play(Player.X, Cell.TopLeft);
        this.Invoking(self => this.game.Play(Player.X, Cell.TopMiddle))
            .Should()
            .ThrowExactly<BadPlayerException>();
    }

    [Fact]
    public void ThePlayerOCanMarkAfterPlayerX()
    {
        this.game.Play(Player.X, Cell.TopLeft);
        var cellMarked = this.game.Play(Player.O, Cell.Right);

        cellMarked
            .Should()
            .BeEquivalentTo([new CellMarked(Player.O, Cell.Right)]);
    }

    [Fact]
    public void APlayerCantMarkACellAlreadyMarked()
    {
        this.game.Play(Player.X, Cell.TopLeft);
        this.game.Play(Player.O, Cell.Middle);
        this.game.Play(Player.X, Cell.BottomRight);

        this.Invoking(self => this.game.Play(Player.O, Cell.TopLeft))
            .Should()
            .ThrowExactly<CellAlreadyMarkedException>();
    }
}