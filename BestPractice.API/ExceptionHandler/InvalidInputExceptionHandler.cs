using BestPractice.API.CustomExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BestPractice.API.ExceptionHandler;

public class InvalidInputExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidInputException invalidInputException) return ValueTask.FromResult(false);
        var problemDetails = new ProblemDetails
        {
            Title = "Invalid input",
            Detail = invalidInputException.Message,
            Status = StatusCodes.Status400BadRequest
        };
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return ValueTask.FromResult(true);
    }
}