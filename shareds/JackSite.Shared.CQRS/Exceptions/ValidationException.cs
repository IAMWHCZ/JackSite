namespace JackSite.Shared.CQRS.Exceptions;

/// <summary>
/// 验证异常
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]> 
        {
            { "Error", [message] }
        };
    }
    
    public ValidationException(IDictionary<string, string[]> errors) : base("验证失败")
    {
        Errors = errors;
    }
    
    public IDictionary<string, string[]> Errors { get; }
}