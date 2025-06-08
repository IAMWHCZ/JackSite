using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Application.CQRS;
using JackSite.Authentication.Base;
using JackSite.Authentication.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JackSite.Authentication.Application.Behaviors;

/// <summary>
/// 事务行为 - 仅应用于命令
/// </summary>
/// <typeparam name="TRequest">请求类型</typeparam>
/// <typeparam name="TResponse">响应类型</typeparam>
public class TransactionBehavior<TRequest, TResponse>(
    IRepository<Entity> repository,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 只对命令应用事务，查询不需要事务
        if (!(request is ICommand || request is ICommand<TResponse>))
        {
            return await next(cancellationToken);
        }

        var requestName = typeof(TRequest).Name;
        
        try
        {
            // 使用仓储的ExecuteInTransactionAsync方法执行事务
            return await repository.ExecuteInTransactionAsync(async () =>
            {
                logger.LogDebug("Begin transaction for {RequestName}", requestName);
                
                try
                {
                    var response = await next(cancellationToken);
                    
                    logger.LogDebug("Committed transaction for {RequestName}", requestName);
                    
                    return response;
                }
                catch (Exception ex)
                {
                    logger.LogDebug("Rolled back transaction for {RequestName} due to: {ExceptionMessage}", 
                        requestName, ex.Message);
                    
                    throw;
                }
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling transaction for {RequestName}", requestName);
            throw;
        }
    }
}