namespace JackSite.Domain.Exceptions;

/// <summary>
/// 表示验证错误的领域对象
/// </summary>
public class ValidationError
{
    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; }
    
    /// <summary>
    /// 错误消息
    /// </summary>
    public string ErrorMessage { get; }
    
    /// <summary>
    /// 错误代码
    /// </summary>
    public string? ErrorCode { get; }
    
    /// <summary>
    /// 尝试的值
    /// </summary>
    public object? AttemptedValue { get; }
    
    /// <summary>
    /// 创建验证错误
    /// </summary>
    /// <param name="propertyName">属性名称</param>
    /// <param name="errorMessage">错误消息</param>
    /// <param name="errorCode">错误代码</param>
    /// <param name="attemptedValue">尝试的值</param>
    public ValidationError(string propertyName, string errorMessage, string? errorCode = null, object? attemptedValue = null)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        AttemptedValue = attemptedValue;
    }
    
    /// <summary>
    /// 返回表示当前对象的字符串
    /// </summary>
    public override string ToString()
    {
        return $"{PropertyName}: {ErrorMessage}";
    }
}