using Domain;
using Domain.Gameplay;
using FluentAssertions;
using UnitTests.Helpers;

namespace UnitTests;

public class CompletedGameTests
{
    protected const DisplayedMark X = DisplayedMark.X;
    protected const DisplayedMark O = DisplayedMark.O;
    protected const DisplayedMark _ = DisplayedMark._;

    [Theory]
    [InlineData(X, X, X, O, O, _, _, _, _)]
    [InlineData(O, O, _, X, X, X, _, _, _)]
    [InlineData(_, _, _, O, O, _, X, X, X)]
    [InlineData(X, O, _, X, O, _, X, _, _)]
    [InlineData(_, X, O, _, X, O, _, X, _)]
    [InlineData(O, _, X, O, _, X, _, _, X)]
    [InlineData(X, O, _, O, X, _, _, _, X)]
    [InlineData(_, O, X, O, X, _, X, _, _)]
    public void Should_make_player_X_win_when_a_line_is_drawn(params DisplayedMark[] cells) => ResultFrom(cells)
        .Should()
        .Be(new WonBy(Player.X));

    [Theory]
    [InlineData(X, _, X, O, O, O, _, X, _)]
    [InlineData(O, _, X, X, O, O, X, X, O)]
    public void Should_make_player_O_win_when_a_line_is_drawn(params DisplayedMark[] cells) => ResultFrom(cells)
        .Should()
        .Be(new WonBy(Player.O));

    [Theory]
    [InlineData(X, O, X, O, O, X, X, X, O)]
    [InlineData(X, O, O, O, X, X, X, X, O)]
    [InlineData(X, O, X, X, X, O, O, X, O)]
    [InlineData(X, O, X, X, O, O, O, X, X)]
    public void Should_end_in_a_draw_when_no_one_wins(params DisplayedMark[] cells) => ResultFrom(cells)
        .Should()
        .Be(new Draw());

    [Fact]
    public void Should_refuse_the_mark_when_the_game_is_already_won()
    {
        var ticTacToe = PlayAGame([
            X, X, X,
            O, O, _,
            _, _, _
        ]);

        this.Invoking(self => ticTacToe.Play(new(Player.O, Cell.Right)))
            .Should()
            .ThrowExactly<GameAlreadyCompletedException>();
    }

    [Fact]
    public void Should_leave_the_result_undetermined_when_another_play_is_possible() => TicTacToe.New()
        .Play(new(Player.X, Cell.Left))
        .Result
        .Should()
        .BeOfType<Undetermined>();

    private static Result ResultFrom(DisplayedMark[] cells) => PlayAGame(cells).Result;

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
