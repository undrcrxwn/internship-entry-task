using FluentAssertions;
using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Exceptions;
using TicTacToe.Domain.Services;

namespace TicTacToe.UnitTests;

public class GameTests
{
    private const byte X = 0;
    private const byte O = 1;

    // Never mistakes a player's mark (zero mistake probability)
    private readonly IMarkFactory _markFactory = new RandomlyWrongMarkFactory(0);

    [Fact(DisplayName = $"Overwriting cell's mark throws {nameof(CellAlreadyOccupiedException)}")]
    public void OverwritingMark_Throws_CellAlreadyOccupiedException()
    {
        // Arrange
        var game = new Game(size: 3, winningStreakLength: 3, playerCount: 2);

        game.MakeMove(1, 1, X, _markFactory);

        // Act
        var act = () => game.MakeMove(1, 1, X, _markFactory);

        // Assert
        act.Should().Throw<CellAlreadyOccupiedException>();
    }

    [Fact(DisplayName = $"Occupying not existing cell throws {nameof(CellOutOfBoundException)}")]
    public void OccupyingNotExistingCell_Throws_CellOutOfBoundException()
    {
        // Arrange
        var game = new Game(size: 3, winningStreakLength: 3, playerCount: 2);

        // Act
        var act = () => game.MakeMove(-1, 1, X, _markFactory);

        // Assert
        act.Should().Throw<CellOutOfBoundException>();
    }

    [Fact(DisplayName = "Winning move updates game winner")]
    public void WinningMove_Updates_GameWinner()
    {
        // X X X
        // O
        //     O

        // Arrange
        var game = new Game(size: 3, winningStreakLength: 3, playerCount: 2);

        // Act
        game.MakeMove(0, 0, X, _markFactory);
        game.MakeMove(1, 0, O, _markFactory);
        game.MakeMove(0, 1, X, _markFactory);
        game.MakeMove(2, 2, O, _markFactory);
        game.MakeMove(0, 2, X, _markFactory);

        // Assert
        game.IsFinished.Should().BeTrue();
        game.WinnerPlayerIndex.Should().Be(X);
    }

    [Fact(DisplayName = "Running out of empty cells results in draw")]
    public void RunningOutOfEmptyCells_ResultsIn_Draw()
    {
        // X X O
        // O O X
        // X O X

        // Arrange
        var game = new Game(size: 3, winningStreakLength: 3, playerCount: 2);

        // Act
        game.MakeMove(0, 0, X, _markFactory);
        game.MakeMove(0, 2, O, _markFactory);
        game.MakeMove(0, 1, X, _markFactory);

        game.MakeMove(1, 0, O, _markFactory);
        game.MakeMove(1, 2, X, _markFactory);
        game.MakeMove(1, 1, O, _markFactory);

        game.MakeMove(2, 0, X, _markFactory);
        game.MakeMove(2, 1, O, _markFactory);
        game.MakeMove(2, 2, X, _markFactory);

        // Assert
        game.IsFinished.Should().BeTrue();
        game.WinnerPlayerIndex.Should().BeNull();
    }
}