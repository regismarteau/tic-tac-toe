using Domain.ValueObjects;

namespace Domain
{
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
            this.CurrentPlayer = ticTacToe.GetNextPlayer();
            this.Opponent = this.CurrentPlayer == Player.X ? Player.O : Player.X;
        }

        public Cell FindBestCellToMark()
        {
            return this.ticTacToe.AvailableCells
                .Select(cell => new
                {
                    Score = this.EvaluateScoreFor(this.ticTacToe, new(this.CurrentPlayer, cell)),
                    Cell = cell
                })
                .OrderByDescending(play => play.Score)
                .First()
                .Cell;
        }

        private Score EvaluateScoreFor(TicTacToe ticTacToe, Mark mark, int depth = 0)
        {
            ticTacToe = ticTacToe.Mark(mark);

            return ticTacToe.Result switch
            {
                WonBy wonBy => wonBy.Player == this.CurrentPlayer ? (Score.Victory - depth) : (Score.Failure + depth),
                Draw => Score.Draw,
                _ => mark.Player == this.Opponent
                    ? this.GetCurrentPlayerNextMarkMaximumScore(ticTacToe, depth)
                    : this.GetOpponentNextMarkMinimumScore(ticTacToe, depth)
            };
        }

        private Score GetCurrentPlayerNextMarkMaximumScore(TicTacToe ticTacToe, int depth)
        {
            var maxScore = Score.Failure;
            foreach (var cell in ticTacToe.AvailableCells)
            {
                var nextMarkScore = this.EvaluateScoreFor(ticTacToe, new(this.CurrentPlayer, cell), depth + 1);
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
                var nextMarkScore = this.EvaluateScoreFor(ticTacToe, new(this.Opponent, cell), depth + 1);
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
}
