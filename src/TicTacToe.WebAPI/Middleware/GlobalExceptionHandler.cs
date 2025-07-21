using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Domain.Exceptions;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace TicTacToe.WebAPI.Middleware;

/// <summary>
/// Handles exceptions thrown in the ASP.NET Core pipeline and writes the details to the HTTP response in accordance with RFC 7807.
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "{ExceptionMessage}", exception.Message);

        var problemDetails = exception switch
        {
            CellOutOfBoundException => new ProblemDetails
            {
                Status = Status400BadRequest,
                Detail = "Cell is out of bound."
            },
            GameNotFoundException => new ProblemDetails
            {
                Status = Status404NotFound,
                Detail = "The requested game doesn't exist."
            },
            MidAirCollisionException => new ProblemDetails
            {
                Status = Status412PreconditionFailed,
                Detail = "The requested resource has changed."
            },
            _ => new ProblemDetails
            {
                Status = Status500InternalServerError,
                Detail = "Something went wrong. Try again later."
            }
        };

        context.Response.ContentType = MediaTypeNames.Application.ProblemJson;
        context.Response.StatusCode = problemDetails.Status ?? Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}