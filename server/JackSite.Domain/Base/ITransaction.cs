namespace JackSite.Domain.Base;

/// <summary>
/// 事务接口
/// </summary>
public interface ITransaction : IDisposable
{
    /// <summary>
    /// 提交事务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    Task CommitAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 回滚事务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}