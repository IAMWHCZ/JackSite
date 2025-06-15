namespace JackSite.Authentication.Application.Exceptions.User;

/// <summary>
/// 表示在用户相关操作过程中发生的异常
/// </summary>
public class UserException : Exception
{
    /// <summary>
    /// 用户相关异常的错误代码
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// 初始化 <see cref="UserException"/> 类的新实例
    /// </summary>
    public UserException() : base("发生用户相关操作异常")
    {
    }

    /// <summary>
    /// 使用指定的错误消息初始化 <see cref="UserException"/> 类的新实例
    /// </summary>
    /// <param name="message">描述错误的消息</param>
    public UserException(string message) : base(message)
    {
    }

    /// <summary>
    /// 使用指定的错误消息和错误代码初始化 <see cref="UserException"/> 类的新实例
    /// </summary>
    /// <param name="message">描述错误的消息</param>
    /// <param name="errorCode">错误代码</param>
    public UserException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// 使用指定的错误消息和对作为此异常原因的内部异常的引用来初始化 <see cref="UserException"/> 类的新实例
    /// </summary>
    /// <param name="message">描述错误的消息</param>
    /// <param name="innerException">导致当前异常的异常</param>
    public UserException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// 使用指定的错误消息、错误代码和对作为此异常原因的内部异常的引用来初始化 <see cref="UserException"/> 类的新实例
    /// </summary>
    /// <param name="message">描述错误的消息</param>
    /// <param name="errorCode">错误代码</param>
    /// <param name="innerException">导致当前异常的异常</param>
    public UserException(string message, string errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}