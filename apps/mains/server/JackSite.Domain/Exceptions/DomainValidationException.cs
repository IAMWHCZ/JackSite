namespace JackSite.Domain.Exceptions;

/// <summary>
/// 领域验证异常
/// </summary>
public class DomainValidationException : Exception
{
    /// <summary>
    /// 验证错误列表
    /// </summary>
    public IReadOnlyCollection<ValidationError> Errors { get; }

    /// <summary>
    /// 创建领域验证异常
    /// </summary>
    /// <param name="message">错误消息</param>
    public DomainValidationException(string message) 
        : base(message)
    {
        Errors = Array.Empty<ValidationError>();
    }

    /// <summary>
    /// 创建领域验证异常
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="errors">验证错误列表</param>
    public DomainValidationException(string message, IEnumerable<ValidationError> errors) 
        : base(message)
    {
        Errors = errors.ToList().AsReadOnly();
    }
}