using Domain;
using Domain.Gameplay;
using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests;

public class PlayTurnTests
{
    [Fact]
    public void ThePlayerXCanMarkACellFirst()
    {
        TicTacToe
            .New()
            .Play(new(Player.X, Cell.TopLeft))
            .Marks
            .Should()
            .BeEquivalentTo([new Mark(Player.X, Cell.TopLeft)]);
    }

    [Fact]
    public void ThePlayerOCantMarkFirst()
    {
        this.Invoking(_ => TicTacToe
            .New()
            .Play(new(Player.O, Cell.TopLeft)))
            .Should()
            .ThrowExactly<BadPlayerException>();
    }

    [Fact]
    public void ThePlayerXCantMarkTwice()
    {
        var ticTacToe = TicTacToe
            .New()
            .Play(new(Player.X, Cell.TopLeft));

        this.Invoking(_ => ticTacToe.Play(new(Player.X, Cell.TopMiddle)))
            .Should()
            .ThrowExactly<BadPlayerException>();
    }

    [Fact]
    public void ThePlayerOCanMarkAfterPlayerX()
    {
        TicTacToe
            .New()
            .Play(new(Player.X, Cell.TopLeft))
            .Play(new(Player.O, Cell.Right))
            .Marks
            .Should()
            .BeEquivalentTo([
                new Mark(Player.X, Cell.TopLeft),
                new Mark(Player.O, Cell.Right)]);
    }

    [Fact]
    public void APlayerCantMarkACellAlreadyMarked()
    {
        var ticTacToe = TicTacToe
            .New()
            .Play(new(Player.X, Cell.TopLeft))
            .Play(new(Player.O, Cell.Right))
            .Play(new(Player.X, Cell.BottomRight));

        this.Invoking(_ => ticTacToe.Play(new(Player.O, Cell.TopLeft)))
            .Should()
            .ThrowExactly<CellAlreadyMarkedException>();
    }
}