namespace TicTacToe.Domain;

public record GameStatus
{
    public required bool IsFinished;
    public required byte? WinnerPlayerIndex;
}