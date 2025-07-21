using TicTacToe.Domain.Entities;
using TicTacToe.Domain.ValueObjects;

namespace TicTacToe.Domain.Abstractions;

public interface IMarkFactory
{
    Mark CreateMarkForPlayer(Game game, byte playerIndex);
}