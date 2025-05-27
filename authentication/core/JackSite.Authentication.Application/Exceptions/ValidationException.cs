using FluentValidation.Results;

namespace JackSite.Authentication.Application.Exceptions;

/// <summary>
/// 验证异常
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// 验证错误集合
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public ValidationException()
        : base("发生一个或多个验证错误")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="failures">验证失败集合</param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.ToArray());
    }
}