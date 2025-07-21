using TicTacToe.Domain.Entities;

namespace TicTacToe.Domain.Abstractions;

public interface IReferee
{
    public GameStatus Judge(Game game);
}