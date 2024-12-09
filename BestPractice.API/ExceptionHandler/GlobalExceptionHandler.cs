using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BestPractice.API.ExceptionHandler;
public class GlobalExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred",
            Detail = exception.Message,
            Status = StatusCodes.Status500InternalServerError
        };
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return ValueTask.FromResult(true);
    }
}
