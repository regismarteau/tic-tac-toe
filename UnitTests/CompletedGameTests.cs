using Domain;
using Domain.Gameplay;
using Domain.ValueObjects;
using FluentAssertions;
using UnitTests.Helpers;

namespace UnitTests;

public class CompletedGameTests
{
    protected const DisplayedMark X = DisplayedMark.X;
    protected const DisplayedMark O = DisplayedMark.O;
    protected const DisplayedMark _ = DisplayedMark._;

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
    public void ThePlayerXWinsTheGameWhenALineIsDrawn(params DisplayedMark[] cells)
    {
        ResultFrom(cells)
            .Should()
            .Be(new WonBy(Player.X));
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
    public void ThePlayerOWinsTheGameWhenALineIsDrawn(params DisplayedMark[] cells)
    {
        ResultFrom(cells)
            .Should()
            .Be(new WonBy(Player.O));
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
    public void ItIsPossibleThatNoOneWins(params DisplayedMark[] cells)
    {
        ResultFrom(cells)
            .Should()
            .Be(new Draw());
    }

    [Fact]
    public void ItIsImpossibleToMarkAnotherCellAfterAVictory()
    {
        var ticTacToe = PlayAGame([
            X, X, X,
            O, O, _,
            _, _, _]);

        this.Invoking(self => ticTacToe.Play(new(Player.O, Cell.Right)))
            .Should()
            .ThrowExactly<GameAlreadyCompletedException>();
    }

    [Fact]
    public void AGameIsNotYetCompletedIfAnotherPlayIsPossible()
    {
        TicTacToe.New()
            .Play(new(Player.X, Cell.Left))
            .Result
            .Should()
            .BeOfType<Undetermined>();
    }

    private static Result ResultFrom(DisplayedMark[] cells)
    {
        return PlayAGame(cells).Result;
    }

    private static TicTacToe PlayAGame(DisplayedMark[] cells)
    {
        var ticTacToe = TicTacToe.New();
        foreach (var move in cells.AsMoves())
        {
            ticTacToe = ticTacToe.Play(new(move.Player, move.Cell));
        }

        return ticTacToe;
    }
}
