using Domain;
using FluentAssertions;

namespace UnitTests;

public class TicTacToeVictoryTests
{
    private readonly TicTacToe ticTacToe;

    public TicTacToeVictoryTests()
    {
        this.ticTacToe = TicTacToe.StartNewGame();
    }

    [Fact]
    public void ThePlayerXCanMarkACellFirst()
    {
        var cellMarked = this.ticTacToe.Mark(Player.X, Cell.TopLeft);
        cellMarked.Should().Be(new CellMarked(Player.X, Cell.TopLeft));
    }
}
