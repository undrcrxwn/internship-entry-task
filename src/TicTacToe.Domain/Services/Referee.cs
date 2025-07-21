using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Domain.Services;

public class Referee : IReferee
{
    private enum StraightOrientation
    {
        Horizontal,
        Vertical
    }

    private enum DiagonalOrientation
    {
        TopRightToBottomLeft,
        TopLeftToBottomRight
    }

    public GameStatus Judge(Game game)
    {
        var winnerIndex =
            GetWinningStraightStreakPlayerIndex(game, StraightOrientation.Horizontal)
            ?? GetWinningStraightStreakPlayerIndex(game, StraightOrientation.Vertical)
            ?? GetWinningDiagonalStreakPlayerIndex(game, DiagonalOrientation.TopRightToBottomLeft)
            ?? GetWinningDiagonalStreakPlayerIndex(game, DiagonalOrientation.TopLeftToBottomRight);

        return new GameStatus
        {
            IsFinished = winnerIndex is not null || game.AreAllCellsOccupied,
            WinnerPlayerIndex = winnerIndex
        };
    }

    private static byte? GetWinningStraightStreakPlayerIndex(Game game, StraightOrientation orientation)
    {
        for (var row = 0; row < game.Size; row++)
        {
            byte? previousMarkPlayerIndex = null;
            var streak = 1;

            for (var column = 0; column < game.Size; column++)
            {
                var mark = orientation switch
                {
                    StraightOrientation.Horizontal => game.GetMarkAt(row, column),
                    StraightOrientation.Vertical => game.GetMarkAt(column, row),
                    _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
                };

                if (previousMarkPlayerIndex == mark.PlayerIndex)
                {
                    streak++;
                }
                else
                {
                    streak = 1;
                    previousMarkPlayerIndex = game.Cells[row].PlayerIndex;
                }
            }

            if (previousMarkPlayerIndex is not null && streak >= game.WinningStreakLength)
                return previousMarkPlayerIndex;
        }

        return null;
    }

    private static byte? GetWinningDiagonalStreakPlayerIndex(Game game, DiagonalOrientation orientation)
    {
        var maxCoordinateSum = game.Size + game.Size - 1;

        for (var coordinateSum = 0; coordinateSum < maxCoordinateSum; coordinateSum++)
        {
            byte? previousMarkPlayerIndex = null;
            var streak = 1;

            // Iterate through cells with row + column = sum
            for (var row = 0; row < game.Size; row++)
            {
                var column = coordinateSum - row;
                if (column < 0 || column >= game.Size) continue;

                var mark = orientation switch
                {
                    DiagonalOrientation.TopRightToBottomLeft => game.GetMarkAt(row, column),
                    DiagonalOrientation.TopLeftToBottomRight => game.GetMarkAt(row, game.Size - column - 1),
                    _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
                };

                if (previousMarkPlayerIndex == mark.PlayerIndex)
                {
                    streak++;
                }
                else
                {
                    streak = 1;
                    previousMarkPlayerIndex = game.Cells[row].PlayerIndex;
                }
            }

            if (previousMarkPlayerIndex is not null && streak >= game.WinningStreakLength)
                return previousMarkPlayerIndex;
        }

        return null;
    }
}