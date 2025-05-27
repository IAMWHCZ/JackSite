using ValidationException = FluentValidation.ValidationException;

namespace JackSite.Authentication.Application.Behaviors;

/// <summary>
/// 请求验证行为
/// </summary>
/// <typeparam name="TRequest">请求类型</typeparam>
/// <typeparam name="TResponse">响应类型</typeparam>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        // 创建验证上下文
        var context = new ValidationContext<TRequest>(request);

        // 执行所有验证器并聚合结果
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // 收集所有验证错误
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // 如果有验证错误，抛出异常
        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }

        // 验证通过，继续处理请求
        return await next(cancellationToken);
    }
}