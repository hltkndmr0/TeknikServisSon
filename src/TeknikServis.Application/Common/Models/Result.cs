namespace TeknikServis.Application.Common.Models;

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static Result<T> Succeed(T data, string? message = null)
    {
        return new Result<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static Result<T> Fail(string error)
    {
        return new Result<T>
        {
            Success = false,
            Errors = new List<string> { error }
        };
    }

    public static Result<T> Fail(List<string> errors)
    {
        return new Result<T>
        {
            Success = false,
            Errors = errors
        };
    }
}