using Microsoft.AspNetCore.Mvc;

namespace BestPractice.API.Services;

public class ServiceResult
{
    public int Status { get; set; }
    public ProblemDetails? ProblemDetails { get; set; }
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public static ServiceResult<T> Success(T data, int status)
    {
        return new ServiceResult<T>
        {
            Data = data,
            Status = status
        };
    }

    public static ServiceResult<T> Fail(string message, int status)
    {
        return new ServiceResult<T>
        {
            Status = status,
            ProblemDetails = new ProblemDetails
            {
                Detail = message
            }
        };
    }
}