using ValidationException = FluentValidation.ValidationException;

namespace JackSite.Shared.CQRS.Middleware;

/// <summary>
/// 异常处理中间件
/// </summary>
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
            logger.LogError(ex, "请求处理过程中发生错误");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };
        
        context.Response.StatusCode = (int)statusCode;
        
        object response;
        if (exception is ValidationException validationException)
        {
            response = new
            {
                title = "验证失败",
                status = (int)statusCode,
                errors = validationException.Errors
            };
        }
        else
        {
            response = new
            {
                title = exception.Message,
                status = (int)statusCode,
                detail = exception.InnerException?.Message
            };
        }
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}