namespace JackSite.Domain.Base;

/// <summary>
/// 工作单元接口
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// 开始事务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>事务对象</returns>
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 保存更改
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>影响的行数</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}