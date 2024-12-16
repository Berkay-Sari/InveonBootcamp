using System.Text.Json.Serialization;

namespace LibraryManagement.Web.Services;

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public List<string>? ErrorMessage { get; set; }
    [JsonIgnore] public bool Succeeded => ErrorMessage == null || ErrorMessage.Count == 0;
    [JsonIgnore] public bool Failed => !Succeeded;
    [JsonIgnore] public int Status { get; set; }

    [JsonIgnore] public string? UrlAsCreated { get; set; }

    public static ServiceResult<T> Success(T data, int status = StatusCodes.Status200OK)
    {
        return new ServiceResult<T>
        {
            Data = data,
            Status = status
        };
    }

    public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCreated)
    {
        return new ServiceResult<T>
        {
            Data = data,
            Status = StatusCodes.Status201Created,
            UrlAsCreated = urlAsCreated
        };
    }


    public static ServiceResult<T> Fail(List<string> errorMessage,
        int status = StatusCodes.Status400BadRequest)
    {
        return new ServiceResult<T>
        {
            ErrorMessage = errorMessage,
            Status = status
        };
    }

    public static ServiceResult<T> Fail(string errorMessage, int status = StatusCodes.Status400BadRequest)
    {
        return new ServiceResult<T>
        {
            ErrorMessage = [errorMessage],
            Status = status
        };
    }
}

public class ServiceResult
{
    public List<string>? ErrorMessage { get; set; }

    [JsonIgnore] public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0;
    [JsonIgnore] public bool IsFail => !IsSuccess;
    [JsonIgnore] public int Status { get; set; }
    public static ServiceResult Success(int status = StatusCodes.Status200OK)
    {
        return new ServiceResult
        {
            Status = status
        };
    }

    public static ServiceResult Fail(List<string> errorMessage,
        int status = StatusCodes.Status400BadRequest)
    {
        return new ServiceResult
        {
            ErrorMessage = errorMessage,
            Status = status
        };
    }

    public static ServiceResult Fail(string errorMessage, int status = StatusCodes.Status400BadRequest)
    {
        return new ServiceResult
        {
            ErrorMessage = [errorMessage],
            Status = status
        };
    }
}