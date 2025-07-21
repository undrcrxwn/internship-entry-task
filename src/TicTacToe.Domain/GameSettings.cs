using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Domain;

public class GameSettings
{
    [Range(3, int.MaxValue)]
    public int BoardSize { get; set; } = 3;
    
    [Range(2, int.MaxValue)]
    public int WinningStreakLength { get; set; } = 3;
    
    [Range(2, byte.MaxValue - 1)]
    public int PlayerCount { get; set; } = 2;
    
    [Range(0, 1)]
    public double WrongMarkProbability = 0.1;
    
    [Range(1, int.MaxValue)]
    public int WrongMarkNthMove = 3;
}