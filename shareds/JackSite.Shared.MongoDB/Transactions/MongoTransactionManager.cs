namespace JackSite.Shared.MongoDB.Transactions;

/// <summary>
/// MongoDB 事务管理器
/// </summary>
public class MongoTransactionManager(IMongoDbClientFactory clientFactory)
{
    /// <summary>
    /// 在事务中执行操作
    /// </summary>
    public async Task ExecuteInTransactionAsync<TResult>(Func<IClientSessionHandle, Task<TResult>> operation,
        TransactionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = clientFactory.GetClient();

        // 创建会话
        using var session = await client.StartSessionAsync(null, cancellationToken);

        // 设置事务选项
        options ??= new TransactionOptions(
            readPreference: ReadPreference.Primary,
            readConcern: ReadConcern.Local,
            writeConcern: WriteConcern.WMajority
        );

        // 开始事务
        session.StartTransaction(options);

        try
        {
            // 执行操作
            var result = await operation(session);

            // 提交事务
            await session.CommitTransactionAsync(cancellationToken);

            return;
        }
        catch (Exception)
        {
            // 回滚事务
            await session.AbortTransactionAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// 在事务中执行操作（无返回值）
    /// </summary>
    public async Task ExecuteInTransactionAsync(
        Func<IClientSessionHandle, Task> operation,
        TransactionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        await ExecuteInTransactionAsync<object>(async (session) =>
        {
            await operation(session);
            return null!;
        }, options, cancellationToken);
    }
}