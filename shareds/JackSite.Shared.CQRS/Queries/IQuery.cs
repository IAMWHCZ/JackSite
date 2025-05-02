using MediatR;

namespace JackSite.Shared.CQRS.Queries;

/// <summary>
/// 查询接口
/// </summary>
public interface IQuery<out TResult> : IRequest<TResult>
{
}