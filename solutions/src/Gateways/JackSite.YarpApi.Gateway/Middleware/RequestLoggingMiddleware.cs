using System.Diagnostics;
using System.Text;
using System.Text.Json;
using JackSite.YarpApi.Gateway.Data;
using JackSite.YarpApi.Gateway.Entities;

namespace JackSite.YarpApi.Gateway.Middleware;

public class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger,
    IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context, GatewayDbContext dbContext)
    {
        var requestLog = new RequestLog
        {
            Path = context.Request.Path,
            Method = context.Request.Method,
            QueryString = context.Request.QueryString.ToString(),
            ClientIp = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers.UserAgent.ToString(),
            RequestTime = DateTime.UtcNow,
            RequestHeaders = JsonSerializer.Serialize(context.Request.Headers
                .Where(h => !h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(h => h.Key, h => h.Value.ToString())),
        };

        // 获取目标服务名称
        var routeConfig = configuration.GetSection("ReverseProxy:Routes").Get<Dictionary<string, object>>();
        var targetService = routeConfig?.Keys
            .FirstOrDefault(route => context.Request.Path.StartsWithSegments($"/{route.Split('-')[0]}-api"));
        requestLog.TargetService = targetService ?? "unknown";

        // 读取请求体
        if (context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            requestLog.RequestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var sw = Stopwatch.StartNew();
        try
        {
            await next(context);

            sw.Stop();
            requestLog.ExecutionTime = sw.ElapsedMilliseconds;
            requestLog.StatusCode = context.Response.StatusCode;
            requestLog.ResponseTime = DateTime.UtcNow;

            // 读取响应头
            requestLog.ResponseHeaders = JsonSerializer.Serialize(context.Response.Headers
                .ToDictionary(h => h.Key, h => h.Value.ToString()));

            // 读取响应体
            responseBody.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(responseBody);
            requestLog.ResponseBody = await reader.ReadToEndAsync();

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            sw.Stop();
            requestLog.ExecutionTime = sw.ElapsedMilliseconds;
            requestLog.StatusCode = 500;
            requestLog.ErrorMessage = ex.Message;
            requestLog.StackTrace = ex.StackTrace;
            requestLog.ResponseTime = DateTime.UtcNow;

            logger.LogError(ex, "Error processing request");
            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
            
            // 异步保存日志
            try
            {
                dbContext.RequestLogs.Add(requestLog);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving request log");
            }
        }
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}