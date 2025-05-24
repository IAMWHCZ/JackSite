using Microsoft.AspNetCore.Http;
using JackSite.Domain.Enums;

namespace JackSite.Infrastructure.Services;

/// <summary>
/// 请求头参数服务实现
/// </summary>
public class RequestHeaderService(
    IHttpContextAccessor httpContextAccessor,
    ILogService logger)
    : IRequestHeaderService
{
    private readonly ILogService _logger = logger.ForContext<RequestHeaderService>();
    
    private BaseHeaderParams? _headerParams;

    /// <inheritdoc />
    public BaseHeaderParams HeaderParams =>
        // 懒加载模式，只在第一次访问时初始化
        _headerParams ??= new BaseHeaderParams
        {
            Language = GetHeaderValue("Accept-Language",LanguageType.Chinese),
            UserId = GetHeaderValue<long>("UserId",(long)0),
            UserName = GetHeaderValue("UserName") ?? string.Empty,
            Email = GetHeaderValue("Email") ?? string.Empty
        };

    /// <inheritdoc />
    public string? GetHeaderValue(string headerName)
    {
        var context = httpContextAccessor.HttpContext;
        if (context != null)
            return context.Request.Headers.TryGetValue(headerName, out var values) ? values.ToString() : null;
        _logger.Warning("尝试获取请求头 {HeaderName} 时HttpContext为空", headerName);
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
            _logger.Warning(ex.Message, "将请求头 {HeaderName} 的值 {HeaderValue} 转换为类型 {TargetType} 失败",
                headerName, value, typeof(T).Name);
            return defaultValue;
        }
    }

    /// <inheritdoc />
    public string? GetClientIpAddress()
    {
        var context = httpContextAccessor.HttpContext;
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
        return !string.IsNullOrEmpty(realIp) ? realIp :
            // 使用连接远程IP
            context.Connection.RemoteIpAddress?.ToString();
    }

    /// <inheritdoc />
    public string? GetUserAgent()
    {
        return GetHeaderValue("User-Agent");
    }
    
    /// <summary>
    /// 刷新请求头参数
    /// </summary>
    public void RefreshHeaderParams()
    {
        _headerParams = new BaseHeaderParams
        {
            Language = GetHeaderValue("Accept-Language",LanguageType.Chinese),
            UserId = GetHeaderValue<long>("UserId",0),
            UserName = GetHeaderValue("UserName") ?? string.Empty,
            Email = GetHeaderValue("Email") ?? string.Empty
            // 可以添加更多从请求头获取的参数
        };
    }
}