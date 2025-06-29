namespace JackSite.Authentication.Application.CQRS;

/// <summary>
/// 查询接口
/// </summary>
/// <typeparam name="TResult">查询返回的结果类型</typeparam>
public interface IQuery<out TResult> : IRequest<TResult>;

/// <summary>
/// 查询处理器接口
/// </summary>
/// <typeparam name="TQuery">要处理的查询类型</typeparam>
/// <typeparam name="TResult">查询返回的结果类型</typeparam>
public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>;

/// <summary>
/// 查询基础实现
/// </summary>
/// <typeparam name="TResult">查询返回的结果类型</typeparam>
public record QueryBase<TResult> : IQuery<TResult>
{
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}
