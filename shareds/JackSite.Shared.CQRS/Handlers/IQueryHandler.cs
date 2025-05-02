using JackSite.Shared.CQRS.Queries;
using MediatR;

namespace JackSite.Shared.CQRS.Handlers;

/// <summary>
/// 查询处理器接口
/// </summary>
public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}