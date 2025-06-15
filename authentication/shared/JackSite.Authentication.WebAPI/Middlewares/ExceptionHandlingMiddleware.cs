using System.Diagnostics;
using System.Text.Json;
using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Application.Results;
using JackSite.Authentication.Entities.Logs;
using ValidationException = FluentValidation.ValidationException;

namespace JackSite.Authentication.WebAPI.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    
    context.Response.ContentType = "application/json";
    var operationRepository = context.RequestServices.GetRequiredService<IRepository<OperationLog>>();
    
    var response = exception switch
    {
        ValidationException validationEx => HandleValidationException(validationEx),
        JackSite.Authentication.Application.Exceptions.ValidationException appValidationEx =>
            HandleAppValidationException(appValidationEx),
        KeyNotFoundException => ApiResult.NotFound(exception.Message),
        UnauthorizedAccessException => ApiResult.Unauthorized(exception.Message),
        _ => HandleUnknownException(exception)
    };

    context.Response.StatusCode = response.Code;
    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    
    stopwatch.Stop();
    
    // 记录异常日志
    await operationRepository.AddAsync(new OperationLog
    {
        ApiName = context.Request.Path,
        Description = exception.Message,
        IsAuthorization = context.User.Identity?.IsAuthenticated ?? false,
        UserId = context.User.FindFirst("UserId")?.Value is not null
            ? long.Parse(context.User.FindFirst("UserId")!.Value)
            : null,
        IpAddress = context.Connection.RemoteIpAddress?.ToString(),
        UserAgent = context.Request.Headers.UserAgent.ToString(),
        Browser = context.Request.Headers.UserAgent.ToString().Contains("Chrome")
            ? "Chrome"
            : context.Request.Headers.UserAgent.ToString().Contains("Firefox")
                ? "Firefox"
                : "Other",
        Os = context.Request.Headers.UserAgent.ToString().Contains("Windows") ? "Windows" :
            context.Request.Headers.UserAgent.ToString().Contains("Linux") ? "Linux" : "Other",
        StatusCode = 500, // 服务器错误
        Exception = exception.ToString(),
        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
    });
}

    private static ApiResult HandleValidationException(ValidationException exception)
    {
        var errors = exception.Errors.Select(e => e.ErrorMessage).ToArray();
        var message = string.Join(" ", errors);
        return ApiResult.Fail(message);
    }

    public ApiResult HandleAppValidationException(
        JackSite.Authentication.Application.Exceptions.ValidationException exception)
    {
        var message = string.Join(" ", exception.Errors.SelectMany(e => e.Value));
        return ApiResult.Fail(message);
    }

    private ApiResult HandleUnknownException(Exception exception)
    {
        logger.LogError(exception, "An unhandled exception occurred");
        return ApiResult.Error("服务器内部错误");
    }
}

// 扩展方法，方便在Program.cs中注册中间件
public static class ExceptionHandlingMiddlewareExtensions
{
    public static WebApplication UseExceptionHandling(this WebApplication builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
        
        return builder;
    }
}