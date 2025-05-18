using System;
using JackSite.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JackSite.Infrastructure.Services;

/// <summary>
/// 请求头参数服务实现
/// </summary>
public class RequestHeaderService : IRequestHeaderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogService _logger;

    public RequestHeaderService(
        IHttpContextAccessor httpContextAccessor,
        ILogService logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger.ForContext<RequestHeaderService>();
    }

    /// <inheritdoc />
    public string? GetHeaderValue(string headerName)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            _logger.Warning("尝试获取请求头 {HeaderName} 时HttpContext为空", headerName);
            return null;
        }

        if (context.Request.Headers.TryGetValue(headerName, out var values))
        {
            return values.ToString();
        }

        return null;
    }

    /// <inheritdoc />
    public T GetHeaderValue<T>(string headerName, T defaultValue)
    {
        var value = GetHeaderValue(headerName);
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "将请求头 {HeaderName} 的值 {HeaderValue} 转换为类型 {TargetType} 失败",
                headerName, value, typeof(T).Name);
            return defaultValue;
        }
    }

    /// <inheritdoc />
    public string? GetClientIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            return null;
        }

        // 尝试从X-Forwarded-For获取
        var forwardedFor = GetHeaderValue("X-Forwarded-For");
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            // X-Forwarded-For可能包含多个IP，取第一个
            return forwardedFor.Split(',')[0].Trim();
        }

        // 尝试从X-Real-IP获取
        var realIp = GetHeaderValue("X-Real-IP");
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        // 使用连接远程IP
        return context.Connection.RemoteIpAddress?.ToString();
    }

    /// <inheritdoc />
    public string? GetUserAgent()
    {
        return GetHeaderValue("User-Agent");
    }
}