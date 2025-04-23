namespace JackSite.Common.Results;

public static class ResultExtensions
{
    public static async Task<Result<TResult>> Map<T, TResult>(
        this Task<Result<T>> resultTask,
        Func<T, TResult> mapper)
    {
        var result = await resultTask;
        return result.IsSuccess && result.Value != null
            ? Result.Success(mapper(result.Value))
            : Result.Failure<TResult>(result.Message ?? "Mapping failed");
    }

    public static async Task<Result<TResult>> Bind<T, TResult>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TResult>>> func)
    {
        var result = await resultTask;
        if (!result.IsSuccess || result.Value == null)
            return Result.Failure<TResult>(result.Message ?? "Binding failed");

        return await func(result.Value);
    }

    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        string errorMessage)
    {
        if (!result.IsSuccess || result.Value == null)
            return result;

        return predicate(result.Value)
            ? result
            : Result.Failure<T>(errorMessage);
    }

    public static async Task<Result<T>> OnSuccess<T>(
        this Task<Result<T>> resultTask,
        Func<T, Task> action)
    {
        var result = await resultTask;
        if (result.IsSuccess && result.Value != null)
            await action(result.Value);

        return result;
    }

    public static async Task<Result<T>> OnFailure<T>(
        this Task<Result<T>> resultTask,
        Func<string?, Task> action)
    {
        var result = await resultTask;
        if (!result.IsSuccess)
            await action(result.Message);

        return result;
    }
}