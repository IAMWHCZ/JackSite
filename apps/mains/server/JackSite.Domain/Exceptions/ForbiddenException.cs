namespace JackSite.Domain.Exceptions;

/// <summary>
/// 禁止访问异常
/// </summary>
public class ForbiddenException : Exception
{
    /// <summary>
    /// 详细错误信息
    /// </summary>
    public string? DetailedMessage { get; }

    /// <summary>
    /// 创建禁止访问异常
    /// </summary>
    /// <param name="message">错误消息</param>
    public ForbiddenException(string message) 
        : base(message)
    {
    }

    /// <summary>
    /// 创建禁止访问异常
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="detailedMessage">详细错误信息</param>
    public ForbiddenException(string message, string? detailedMessage) 
        : base(message)
    {
        DetailedMessage = detailedMessage;
    }

    /// <summary>
    /// 创建禁止访问异常
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="innerException">内部异常</param>
    public ForbiddenException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}