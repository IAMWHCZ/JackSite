namespace JackSite.Domain.Exceptions;

/// <summary>
/// 资源未找到异常
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// 详细错误信息
    /// </summary>
    public string? DetailedMessage { get; }

    /// <summary>
    /// 创建资源未找到异常
    /// </summary>
    /// <param name="message">错误消息</param>
    public NotFoundException(string message) 
        : base(message)
    {
    }

    /// <summary>
    /// 创建资源未找到异常
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="detailedMessage">详细错误信息</param>
    public NotFoundException(string message, string? detailedMessage) 
        : base(message)
    {
        DetailedMessage = detailedMessage;
    }

    /// <summary>
    /// 创建资源未找到异常
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="innerException">内部异常</param>
    public NotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
    
    /// <summary>
    /// 创建特定类型资源未找到异常
    /// </summary>
    /// <typeparam name="TResource">资源类型</typeparam>
    /// <param name="id">实体ID</param>
    /// <returns>资源未找到异常</returns>
    public static NotFoundException For<TResource>(object id)
    {
        return new NotFoundException($"未找到 ID 为 {id} 的 {typeof(TResource).Name} 实体");
    }
}