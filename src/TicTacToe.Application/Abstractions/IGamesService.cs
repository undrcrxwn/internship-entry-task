using TicTacToe.Application.Models;

namespace TicTacToe.Application.Abstractions;

public interface IGamesService
{
    public Task<GameResponse> CreateAsync();
    public Task<GameResponse> PlaceMarkAsync(Guid gameId, int row, int column, byte mark, string? expectedEntityTag = null);
}