using Domain;
using Domain.Gameplay;
using FluentAssertions;

namespace UnitTests;

public class PlayTurnTests
{
    [Fact]
    public void Should_let_player_X_mark_a_cell_when_the_game_starts() => TicTacToe
        .New()
        .Play(new(Player.X, Cell.TopLeft))
        .Marks
        .Should()
        .BeEquivalentTo([new Mark(Player.X, Cell.TopLeft)]);

    [Fact]
    public void Should_refuse_player_O_when_playing_first() => this.Invoking(_ => TicTacToe
            .New()
            .Play(new(Player.O, Cell.TopLeft)))
        .Should()
        .ThrowExactly<BadPlayerException>();

    [Fact]
    public void Should_refuse_player_X_when_playing_twice_in_a_row()
    {
        var ticTacToe = TicTacToe
            .New()
            .Play(new(Player.X, Cell.TopLeft));

        this.Invoking(_ => ticTacToe.Play(new(Player.X, Cell.TopMiddle)))
            .Should()
            .ThrowExactly<BadPlayerException>();
    }

    [Fact]
    public void Should_let_player_O_mark_a_cell_when_player_X_has_played() => TicTacToe
        .New()
        .Play(new(Player.X, Cell.TopLeft))
        .Play(new(Player.O, Cell.Right))
        .Marks
        .Should()
        .BeEquivalentTo([
            new(Player.X, Cell.TopLeft),
            new Mark(Player.O, Cell.Right)
        ]);

    [Fact]
    public void Should_refuse_the_mark_when_the_cell_is_already_marked()
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
