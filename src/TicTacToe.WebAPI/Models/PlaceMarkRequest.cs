namespace TicTacToe.WebAPI.Models;

public record PlaceMarkRequest(int Row, int Column, byte PlayerIndex);