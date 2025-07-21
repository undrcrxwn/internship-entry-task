using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using TicTacToe.Application.Abstractions;
using TicTacToe.Application.Models;
using TicTacToe.WebAPI.Models;

namespace TicTacToe.WebAPI.Controllers;

[ApiController, Route("[controller]")]
public class GamesController(IGamesService games) : ControllerBase
{
    [HttpPost]
    public async Task<GameResponse> CreateAsync() => await games.CreateAsync();

    [HttpPost("{gameId:guid}/moves")]
    public async Task<GameResponse> PlaceMarkAsync(Guid gameId, PlaceMarkRequest request)
    {
        var expectedEntityTag = Request.Headers.IfMatch.SingleOrDefault();

        var game = await games.PlaceMarkAsync(gameId, request.Row, request.Column, request.PlayerIndex, expectedEntityTag);

        Response.Headers.ETag = new StringValues(game.EntityTag);
        return game;
    }
}