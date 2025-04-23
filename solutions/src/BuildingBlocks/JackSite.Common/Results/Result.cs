namespace JackSite.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Message { get; }
    public Dictionary<string, string[]>? Errors { get; }

    protected Result(bool isSuccess, string? message = null, Dictionary<string, string[]>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
    }

    public static Result Success(string? message = null)
        => new(true, message);

    public static Result<T> Success<T>(T value, string? message = null)
        => new(value, true, message);

    public static Result Failure(string message)
        => new(false, message);

    public static Result Failure(Dictionary<string, string[]> errors)
        => new(false, errors: errors);

    public static Result<T> Failure<T>(string message)
        => new(default, false, message);

    public static Result<T> Failure<T>(Dictionary<string, string[]> errors)
        => new(default, false, errors: errors);
}

public class Result<T> : Result
{
    public T? Value { get; }

    internal Result(T? value, bool isSuccess, string? message = null, Dictionary<string, string[]>? errors = null)
        : base(isSuccess, message, errors)
    {
        Value = value;
    }

    public static implicit operator Result<T>(T value)
        => Success(value);
}