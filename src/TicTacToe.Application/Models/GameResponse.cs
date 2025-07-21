using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Models;

public class GameResponse
{
    public Guid Id { get; set; }
    public byte? NextPlayer { get; set; }
    public string EntityTag { get; set; }
    public byte?[][] Cells { get; set; }

    public static GameResponse FromEntity(Game game) => new()
    {
        Id = game.Id,
        NextPlayer = game.GetNextMovePlayerIndex(),
        EntityTag = game.EntityTag.ToString(),
        Cells = game.Cells
            .Select(cell => cell.PlayerIndex)
            .Chunk(game.Size)
            .ToArray()
    };
}