using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Exceptions;
using TicTacToe.Domain.Services;
using TicTacToe.Domain.ValueObjects;

namespace TicTacToe.Domain.Entities;

public class Game
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public int Size { get; set; }
    public int WinningStreakLength { get; set; }
    public int PlayerCount { get; set; }
    public int MoveCount { get; set; }
    public byte? LastMovePlayerIndex { get; set; }
    public byte? WinnerPlayerIndex { get; set; }
    public bool IsFinished { get; private set; }
    public int EntityTag { get; set; }
    public IList<Mark> Cells { get; set; }
    public bool AreAllCellsOccupied => MoveCount == Size * Size;
    private readonly IReferee _referee = new Referee();

    public Game(int size, int winningStreakLength, int playerCount)
    {
        Size = size;
        WinningStreakLength = winningStreakLength;
        PlayerCount = playerCount;
        Cells = Enumerable.Repeat(Mark.Empty, Size * Size).ToArray();
    }

    public byte? GetNextMovePlayerIndex()
    {
        if (IsFinished) return null;
        return LastMovePlayerIndex is not null
            ? (byte)((LastMovePlayerIndex.Value + 1) % PlayerCount)
            : (byte)0;
    }

    /// <exception cref="CellOutOfBoundException">Is thrown when the specified coordinates are invalid.</exception>
    /// <exception cref="CellAlreadyOccupiedException">Is thrown when the requested cell is already occupied with a non-empty <see cref="Mark"/>.</exception>
    public Mark GetMarkAt(int row, int column)
    {
        var cellIndex = GetCellIndex(row, column);
        return Cells[cellIndex];
    }

    /// <exception cref="CellOutOfBoundException">Is thrown when the specified coordinates are invalid.</exception>
    /// <exception cref="CellAlreadyOccupiedException">Is thrown when the requested cell is already occupied with a non-empty <see cref="Mark"/>.</exception>
    public void MakeMove(int row, int column, byte playerIndex, IMarkFactory markFactory)
    {
        var mark = markFactory.CreateMarkForPlayer(this, playerIndex);
        OccupyCell(row, column, mark);
        LastMovePlayerIndex = playerIndex;
        MoveCount++;
        EntityTag = HashCode.Combine(EntityTag, row, column, playerIndex, mark.PlayerIndex);

        var status = _referee.Judge(this);
        WinnerPlayerIndex = status.WinnerPlayerIndex;
        IsFinished = status.IsFinished;
    }

    private void OccupyCell(int row, int column, Mark mark)
    {
        var cellIndex = GetCellIndex(row, column);

        if (Cells[cellIndex].IsOccupied)
            throw new CellAlreadyOccupiedException(Cells[cellIndex]);

        Cells[cellIndex] = mark;
    }

    private int GetCellIndex(int row, int column)
    {
        if (row < 0 || row >= Size || column < 0 || column >= Size)
            throw new CellOutOfBoundException();

        return row * Size + column;
    }
}