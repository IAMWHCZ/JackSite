using JackSite.Authentication.Base;

namespace JackSite.Authentication.Interfaces.Repositories;

/// <summary>
/// BaseLogEntity 仓储接口
/// </summary>
public interface IBaseLogEntityRepository : IRepository<BaseLogEntity>
{
    // 可在此添加BaseLogEntity专属方法

    /// <summary>
    /// 获取指定状态码的日志数量
    /// </summary>
    Task<int> CountByStatusCodeAsync(int statusCode);

    /// <summary>
    /// 获取最近N条异常日志
    /// </summary>
    Task<List<BaseLogEntity>> GetRecentExceptionsAsync(int count);

    /// <summary>
    /// 创建一个成功日志
    /// </summary>
    Task<BaseLogEntity> CreateSuccessLogAsync(string? message = null, long elapsedMilliseconds = 0);

    /// <summary>
    /// 创建一个失败日志
    /// </summary>
    Task<BaseLogEntity> CreateFailureLogAsync(string? exception, long elapsedMilliseconds = 0);

    /// <summary>
    /// 创建一个自定义状态码的日志
    /// </summary>
    Task<BaseLogEntity> CreateLogAsync(int statusCode, string? exception = null, long elapsedMilliseconds = 0);
}
