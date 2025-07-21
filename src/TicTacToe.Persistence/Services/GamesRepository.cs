using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.Services;

public class GamesRepository(AppDbContext context) : IGamesRepository
{
    public async Task<Game?> GetByIdAsync(Guid id) => await context.Games.FindAsync(id);
    public async Task AddAsync(Game game) => await context.Games.AddAsync(game);

    public async Task UpdateAsync(Game game)
    {
        context.Games.Update(game);
        await context.SaveChangesAsync();
    }
}