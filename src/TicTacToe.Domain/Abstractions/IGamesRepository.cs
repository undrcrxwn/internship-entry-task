using TicTacToe.Domain.Entities;

namespace TicTacToe.Domain.Abstractions;

public interface IGamesRepository
{
    public Task<Game?> GetByIdAsync(Guid id);
    public Task AddAsync(Game game);
    public Task UpdateAsync(Game game);
}