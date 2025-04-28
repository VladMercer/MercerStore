namespace MercerStore.Web.Infrastructure.Helpers;

public class Result<T>
{
    private Result(T data)
    {
        IsSuccess = true;
        Data = data;
    }

    private Result(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }

    public static Result<T> Success(T data)
    {
        return new Result<T>(data);
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(errorMessage);
    }
}