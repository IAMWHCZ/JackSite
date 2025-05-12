using System.Text;
using FluentValidation;
using FluentValidation.Results;
using JackSite.Domain.Exceptions;
using JackSite.Domain.Services;

namespace JackSite.Application.Behaviors;

/// <summary>
/// MediatR 请求验证行为
/// </summary>
/// <typeparam name="TRequest">请求类型</typeparam>
/// <typeparam name="TResponse">响应类型</typeparam>
public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogService? logger = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogService? _logger = logger?.ForContext<ValidationBehavior<TRequest, TResponse>>();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        var requestName = requestType.Name;

        // 如果没有验证器，直接处理请求
        if (!validators.Any())
        {
            _logger?.Debug("没有为请求 {RequestName} 找到验证器，跳过验证", requestName);
            return await next(cancellationToken);
        }

        _logger?.Debug("开始验证请求 {RequestName}，找到 {ValidatorCount} 个验证器",
            requestName, validators.Count());

        // 创建验证上下文
        var context = new ValidationContext<TRequest>(request);

        try
        {
            // 执行所有验证器并聚合结果
            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // 收集所有验证失败
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // 如果有验证失败，抛出异常
            if (failures.Count > 0)
            {
                throw CreateValidationException(failures, requestName);
            }

            _logger?.Information("请求 {RequestName} 验证通过", requestName);

            // 验证通过，继续处理请求
            return await next(cancellationToken);
        }
        catch (ValidationException validationEx)
        {
            // 已经是验证异常，直接记录并转换为领域异常
            LogValidationErrors(validationEx.Errors, requestName);

            // 将 FluentValidation 异常转换为领域异常
            var domainErrors = ConvertToDomainErrors(validationEx.Errors);
            throw new DomainValidationException(validationEx.Message, domainErrors);
        }
        catch (Exception ex) when (ex is not ValidationException && ex is not DomainValidationException)
        {
            // 验证过程中发生其他异常
            _logger?.Error(ex, "验证请求 {RequestName} 时发生异常: {ErrorMessage}",
                requestName, ex.Message);

            // 包装为领域验证异常
            throw new DomainValidationException($"验证请求 {requestName} 时发生异常: {ex.Message}");
        }
    }

    /// <summary>
    /// 创建适当的验证异常
    /// </summary>
    private Exception CreateValidationException(IEnumerable<ValidationFailure> failures, string requestName)
    {
        // 收集验证错误
        var validationFailures = failures as ValidationFailure[] ?? failures.ToArray();
        var errorMessages = validationFailures
            .Select(f => $"{f.PropertyName}: {f.ErrorMessage}")
            .ToList();

        // 构建详细错误消息
        var errorMessageBuilder = new StringBuilder();
        errorMessageBuilder.AppendLine($"请求 {requestName} 验证失败。错误详情:");

        foreach (var error in errorMessages)
        {
            errorMessageBuilder.AppendLine($"- {error}");
        }

        // 记录验证错误
        LogValidationErrors(validationFailures, requestName);

        // 将 FluentValidation 错误转换为领域错误
        var domainErrors = ConvertToDomainErrors(validationFailures);

        // 根据验证错误类型创建不同的异常
        if (validationFailures.Any(f => f.ErrorCode == "NotFound"))
        {
            // 如果包含 NotFound 错误，抛出 NotFoundException
            var notFoundErrors = validationFailures.Where(f => f.ErrorCode == "NotFound").ToList();
            var notFoundMessage = string.Join(", ", notFoundErrors.Select(f => f.ErrorMessage));

            return new NotFoundException(notFoundMessage, errorMessageBuilder.ToString());
        }

        if (validationFailures.Any(f => f.ErrorCode == "Unauthorized"))
        {
            // 如果包含 Unauthorized 错误，抛出 UnauthorizedException
            var unauthorizedErrors = validationFailures.Where(f => f.ErrorCode == "Unauthorized").ToList();
            var unauthorizedMessage = string.Join(", ", unauthorizedErrors.Select(f => f.ErrorMessage));

            return new UnauthorizedException(unauthorizedMessage, errorMessageBuilder.ToString());
        }

        if (validationFailures.Any(f => f.ErrorCode == "Forbidden"))
        {
            // 如果包含 Forbidden 错误，抛出 ForbiddenException
            var forbiddenErrors = validationFailures.Where(f => f.ErrorCode == "Forbidden").ToList();
            var forbiddenMessage = string.Join(", ", forbiddenErrors.Select(f => f.ErrorMessage));

            return new ForbiddenException(forbiddenMessage, errorMessageBuilder.ToString());
        }

        if (validationFailures.All(f => f.ErrorCode != "Conflict"))
            return new DomainValidationException(errorMessageBuilder.ToString(), domainErrors);
        {
            // 如果包含 Conflict 错误，抛出 ConflictException
            var conflictErrors = validationFailures.Where(f => f.ErrorCode == "Conflict").ToList();
            var conflictMessage = string.Join(", ", conflictErrors.Select(f => f.ErrorMessage));

            return new ConflictException(conflictMessage, errorMessageBuilder.ToString());
        }

    }

    /// <summary>
    /// 将 FluentValidation 错误转换为领域错误
    /// </summary>
    private static IEnumerable<ValidationError> ConvertToDomainErrors(IEnumerable<ValidationFailure> failures)
    {
        return failures.Select(failure => new ValidationError(
            failure.PropertyName,
            failure.ErrorMessage,
            failure.ErrorCode,
            failure.AttemptedValue
        )).ToList();
    }

    /// <summary>
    /// 记录验证错误
    /// </summary>
    private void LogValidationErrors(IEnumerable<ValidationFailure> failures, string requestName)
    {
        if (_logger == null) return;

        var validationFailures = failures as ValidationFailure[] ?? failures.ToArray();
        var errorMessages = validationFailures
            .Select(f => $"{f.PropertyName}: {f.ErrorMessage} (ErrorCode: {f.ErrorCode})")
            .ToList();

        _logger.Warning("请求 {RequestName} 验证失败，共有 {ErrorCount} 个错误: {ValidationErrors}",
            requestName, errorMessages.Count, string.Join("; ", errorMessages));

        // 记录详细的验证错误
        foreach (var failure in validationFailures)
        {
            _logger.ForContext("PropertyName", failure.PropertyName)
                .ForContext("ErrorCode", failure.ErrorCode)
                .ForContext("AttemptedValue", failure.AttemptedValue)
                .ForContext("CustomState", failure.CustomState)
                .ForContext("Severity", failure.Severity)
                .Debug("验证错误详情: {ErrorMessage}", failure.ErrorMessage);
        }
    }
}