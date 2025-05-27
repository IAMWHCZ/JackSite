using System.Text.Json;
using JackSite.Authentication.Application.Results;
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
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            ValidationException validationEx => HandleValidationException(validationEx),
            JackSite.Authentication.Application.Exceptions.ValidationException appValidationEx => HandleAppValidationException(appValidationEx),
            KeyNotFoundException => ApiResult.NotFound(exception.Message),
            UnauthorizedAccessException => ApiResult.Unauthorized(exception.Message),
            _ => HandleUnknownException(exception)
        };

        context.Response.StatusCode = response.Code;
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private ApiResult HandleValidationException(ValidationException exception)
    {
        var errors = exception.Errors.Select(e => e.ErrorMessage).ToArray();
        var message = string.Join(" ", errors);
        return ApiResult.Fail(message, 400);
    }

    public ApiResult HandleAppValidationException(JackSite.Authentication.Application.Exceptions.ValidationException exception)
    {
        var message = string.Join(" ", exception.Errors.SelectMany(e => e.Value));
        return ApiResult.Fail(message, 400);
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
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}