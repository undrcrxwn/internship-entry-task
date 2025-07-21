using Microsoft.Extensions.Options;
using TicTacToe.Application.Abstractions;
using TicTacToe.Application.Models;
using TicTacToe.Domain;
using TicTacToe.Domain.Abstractions;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Exceptions;

namespace TicTacToe.Application.Services;

public class GamesService(IGamesRepository games, IMarkFactory marks, IOptions<GameSettings> settings) : IGamesService
{
    private readonly GameSettings _settings = settings.Value;

    public async Task<GameResponse> CreateAsync()
    {
        var game = new Game(_settings.BoardSize, _settings.WinningStreakLength, _settings.PlayerCount);
        await games.AddAsync(game);
        return GameResponse.FromEntity(game);
    }

    public async Task<GameResponse> PlaceMarkAsync(Guid gameId, int row, int column, byte playerIndex, string? expectedEntityTag)
    {
        var game = await games.GetByIdAsync(gameId) ?? throw new GameNotFoundException();

        if (expectedEntityTag is not null && expectedEntityTag != game.EntityTag.ToString())
            throw new MidAirCollisionException();

        game.MakeMove(row, column, playerIndex, marks);
        await games.UpdateAsync(game);
        return GameResponse.FromEntity(game);
    }
}