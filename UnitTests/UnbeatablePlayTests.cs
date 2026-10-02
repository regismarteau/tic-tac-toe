using Domain.Gameplay;
using Domain.UnbeatableComputer;
using FluentAssertions;

namespace UnitTests;

public class UnbeatablePlayTests
{
    [Fact]
    public void Should_never_let_player_X_win_when_player_O_plays_the_best_move_every_time()
    {
        var unexpectedResult = new WonBy(Player.X);
        var allGameResults = AllPossibleGameResults(TicTacToe.New()).ToList();
        allGameResults
            .Should()
            .NotContain(unexpectedResult, "There are {0} X Player victories among {1} different games",
                allGameResults.Count(result => result == unexpectedResult),
                allGameResults.Count);
    }

    private static IEnumerable<Completed> AllPossibleGameResults(TicTacToe ticTacToe) => ticTacToe.AvailableCells.SelectMany(cell => PlayATurn(ticTacToe, cell));

    private static IEnumerable<Completed> PlayATurn(TicTacToe ticTacToe, Cell cell)
    {
        ticTacToe = ticTacToe.Play(new(Player.X, cell));

        if (ticTacToe.Result is Completed)
        {
            return [(Completed)ticTacToe.Result];
        }

        var bestCell = new UnbeatablePlayFinder(ticTacToe).FindBestCellToMark();
        ticTacToe = ticTacToe.Play(new(Player.O, bestCell));
        if (ticTacToe.Result is Completed)
        {
            return [(Completed)ticTacToe.Result];
        }

        return AllPossibleGameResults(ticTacToe);
    }
}
