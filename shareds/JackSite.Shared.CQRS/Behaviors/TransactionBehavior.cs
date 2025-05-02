namespace JackSite.Shared.CQRS.Behaviors;

/// <summary>
/// 事务行为
/// </summary>
public class TransactionBehavior<TRequest, TResponse>(
    DbContext dbContext,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        try
        {
            // 只有命令才需要事务
            if (request is ICommand)
            {
                logger.LogInformation("开始事务 {RequestName}", requestName);
                
                // 创建事务
                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                
                // 执行处理器
                var response = await next(cancellationToken);
                
                // 提交事务
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                
                logger.LogInformation("提交事务 {RequestName}", requestName);
                
                return response;
            }
            else
            {
                // 查询不需要事务
                return await next(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "处理事务 {RequestName} 时发生错误", requestName);
            throw;
        }
    }
}