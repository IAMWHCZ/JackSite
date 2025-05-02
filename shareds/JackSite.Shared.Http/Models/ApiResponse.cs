using System.Text.Json.Serialization;

namespace JackSite.Shared.Http.Models;

/// <summary>
/// API 响应模型
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ApiResponse() { }
    
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="success">是否成功</param>
    /// <param name="message">消息</param>
    public ApiResponse(T? data, bool success = true, string? message = null)
    {
        Data = data;
        Success = success;
        Message = message;
    }

    /// <summary>
    /// 是否成功
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; } = true;

    /// <summary>
    /// 消息
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}