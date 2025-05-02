

using ValidationException = FluentValidation.ValidationException;

namespace JackSite.Shared.CQRS.Behaviors;

/// <summary>
/// 验证行为
/// </summary>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        // 创建验证上下文
        var context = new ValidationContext<TRequest>(request);

        // 执行所有验证器
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // 合并验证错误
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // 如果有验证错误，抛出异常
        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await next(cancellationToken);
    }
}