using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JackSite.Shared.Http.Models;

/// <summary>
/// API 错误响应
/// </summary>
public class ApiError
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ApiError() { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="exception">异常信息</param>
    /// <param name="trackId">跟踪ID</param>
    public ApiError(string message, string? exception = null, long? trackId = null)
    {
        Message = message;
        Exception = exception;
        TrackId = trackId ?? GetTraceId();
    }

    /// <summary>
    /// 跟踪ID
    /// </summary>
    [JsonPropertyName("trackId")]
    public long TrackId { get; set; } = GetTraceId();

    /// <summary>
    /// 错误消息
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 异常信息
    /// </summary>
    [JsonPropertyName("exception")]
    public string? Exception { get; set; }

    /// <summary>
    /// 获取跟踪ID
    /// </summary>
    /// <returns>跟踪ID</returns>
    private static long GetTraceId()
    {
        // 尝试获取当前活动的跟踪ID
        var activity = Activity.Current;
        if (activity != null)
        {
            return Convert.ToInt64(activity.TraceId);
        }
        
        // 如果没有活动，则使用时间戳作为备用
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}