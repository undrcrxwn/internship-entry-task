using TicTacToe.Domain.ValueObjects;

namespace TicTacToe.Domain.Exceptions;

public class CellAlreadyOccupiedException(Mark mark) : Exception
{
    public Mark Mark => mark;
}