using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.ValueObjects;

namespace TicTacToe.Domain.Services;

/// <summary>
/// Picks the next player's mark on every <paramref name="n"/>th move with a given <paramref name="probability"/>.
/// </summary>
public class RandomlyWrongMarkFactory(double probability, int n = 1) : IMarkFactory
{
    public Mark CreateMarkForPlayer(Game game, byte playerIndex)
    {
        var isNthMove = (game.MoveCount + 1) % n == 0;
        var isUnluckyMove = Random.Shared.NextDouble() < probability;
        var nextPlayerIndex = (byte)((playerIndex + 1) % game.PlayerCount);

        return Mark.FromPlayerIndex(isNthMove && isUnluckyMove
            ? nextPlayerIndex
            : playerIndex);
    }
}