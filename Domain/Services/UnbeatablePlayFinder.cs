using Domain.ValueObjects;

namespace Domain.Services;

public class UnbeatablePlayFinder
{
    private enum Score
    {
        Failure = -10,
        Draw = 0,
        Victory = 10
    }
    private readonly Player CurrentPlayer;
    private readonly Player Opponent;
    private readonly TicTacToe ticTacToe;

    public UnbeatablePlayFinder(TicTacToe ticTacToe)
    {
        this.ticTacToe = ticTacToe;
        CurrentPlayer = ticTacToe.NextPlayer;
        Opponent = CurrentPlayer == Player.X ? Player.O : Player.X;
    }

    public Cell FindBestCellToMark()
    {
        return ticTacToe.AvailableCells
            .Select(cell => new
            {
                Score = EvaluateScoreFor(ticTacToe, new(CurrentPlayer, cell)),
                Cell = cell
            })
            .OrderByDescending(play => play.Score)
            .First()
            .Cell;
    }

    private Score EvaluateScoreFor(TicTacToe ticTacToe, Mark mark, int depth = 0)
    {
        ticTacToe = ticTacToe.Play(mark);

        return ticTacToe.Result switch
        {
            WonBy wonBy => wonBy.Player == CurrentPlayer ? Score.Victory - depth : Score.Failure + depth,
            Draw => Score.Draw,
            _ => mark.Player == Opponent
                ? GetCurrentPlayerNextMarkMaximumScore(ticTacToe, depth)
                : GetOpponentNextMarkMinimumScore(ticTacToe, depth)
        };
    }

    private Score GetCurrentPlayerNextMarkMaximumScore(TicTacToe ticTacToe, int depth)
    {
        var maxScore = Score.Failure;
        foreach (var cell in ticTacToe.AvailableCells)
        {
            var nextMarkScore = EvaluateScoreFor(ticTacToe, new(CurrentPlayer, cell), depth + 1);
            if (nextMarkScore > Score.Draw)
            {
                return nextMarkScore;
            }

            if (maxScore < nextMarkScore)
            {
                maxScore = nextMarkScore;
            }
        }

        return maxScore;
    }

    private Score GetOpponentNextMarkMinimumScore(TicTacToe ticTacToe, int depth)
    {
        var minScore = Score.Victory;
        foreach (var cell in ticTacToe.AvailableCells)
        {
            var nextMarkScore = EvaluateScoreFor(ticTacToe, new(Opponent, cell), depth + 1);
            if (nextMarkScore < Score.Draw)
            {
                return nextMarkScore;
            }

            if (minScore > nextMarkScore)
            {
                minScore = nextMarkScore;
            }
        }

        return minScore;
    }
}
