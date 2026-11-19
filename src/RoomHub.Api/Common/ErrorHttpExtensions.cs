using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Common;

// The single place where an Error becomes an HTTP response (ProblemDetails).
public static class ErrorHttpExtensions
{
    public static ProblemHttpResult ToProblem(this Error error)
    {
        var status = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return TypedResults.Problem(
            detail: error.Message,
            statusCode: status,
            title: error.Code,
            extensions: new Dictionary<string, object?> { ["code"] = error.Code });
    }
}