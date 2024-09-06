using Domain;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;
using UnitTests.Helpers;

namespace UnitTests;

public class CompletedGameTests
{
    protected const DisplayedCell X = DisplayedCell.X;
    protected const DisplayedCell O = DisplayedCell.O;
    protected const DisplayedCell _ = DisplayedCell._;
    private readonly Game game;

    public CompletedGameTests()
    {
        this.game = Game.Start();
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
    public void ThePlayerXWinsTheGameWhenALineIsDrawn(params DisplayedCell[] cells)
    {
        this.PlayAGame(cells)
            .Should()
            .Be(new GameWon(Player.X));
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
    public void ThePlayerOWinsTheGameWhenALineIsDrawn(params DisplayedCell[] cells)
    {
        this.PlayAGame(cells)
            .Should()
            .Be(new GameWon(Player.O));
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
    public void ItIsPossibleThatNoOneWins(params DisplayedCell[] cells)
    {
        this.PlayAGame(cells)
            .Should()
            .Be(new GameResultedAsADraw());
    }

    [Fact]
    public void ItIsImpossibleToMarkAnotherCellAfterAVictory()
    {
        this.PlayAGame([
            X, X, X,
            O, O, _,
            _, _, _]);

        this.Invoking(self => self.game.Play(Player.O, Cell.Right))
            .Should()
            .ThrowExactly<GameAlreadyCompletedException>();
    }

    [Fact]
    public void AGameIsNotYetCompletedIfAnotherPlayIsPossible()
    {
        this.game
            .Play(Player.X, Cell.Left)
            .OfType<GameCompleted>()
            .Should()
            .BeEmpty();
    }

    private GameCompleted? PlayAGame(DisplayedCell[] cells)
    {
        foreach (var play in cells.AsPlays())
        {
            var gameCompleted = this.game
                .Play(play.Player, play.Cell)
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
