using Domain.ValueObjects;

namespace Domain
{
    public class UnbeatableMoveFinder
    {
        private enum Score
        {
            Failure = -1,
            Draw = 0,
            Victory = 1
        }
        private readonly Player CurrentPlayer;
        private readonly Player Opponent;
        private readonly TicTacToe ticTacToe;

        public UnbeatableMoveFinder(TicTacToe ticTacToe)
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

        private Score EvaluateScoreFor(TicTacToe ticTacToe, Mark mark)
        {
            ticTacToe = ticTacToe.Mark(mark);

            return ticTacToe.Result switch
            {
                WonBy wonBy => wonBy.Player == this.CurrentPlayer ? Score.Victory : Score.Failure,
                Draw => Score.Draw,
                _ => mark.Player == this.Opponent
                    ? this.GetCurrentPlayerNextMarkMaximumScore(ticTacToe)
                    : this.GetOpponentNextMarkMinimumScore(ticTacToe)
            };
        }

        private Score GetCurrentPlayerNextMarkMaximumScore(TicTacToe ticTacToe)
        {
            var maxScore = Score.Failure;
            foreach (var cell in ticTacToe.AvailableCells)
            {
                var nextMarkScore = this.EvaluateScoreFor(ticTacToe, new(this.CurrentPlayer, cell));
                if (nextMarkScore == Score.Victory)
                {
                    return Score.Victory;
                }

                if (nextMarkScore == Score.Draw)
                {
                    maxScore = Score.Draw;
                }
            }

            return maxScore;
        }

        private Score GetOpponentNextMarkMinimumScore(TicTacToe ticTacToe)
        {
            var minScore = Score.Victory;
            foreach (var cell in ticTacToe.AvailableCells)
            {
                var nextMarkScore = this.EvaluateScoreFor(ticTacToe, new(this.Opponent, cell));
                if (nextMarkScore == Score.Failure)
                {
                    return Score.Failure;
                }

                if (nextMarkScore == Score.Draw)
                {
                    minScore = Score.Draw;
                }
            }

            return minScore;
        }
    }
}
