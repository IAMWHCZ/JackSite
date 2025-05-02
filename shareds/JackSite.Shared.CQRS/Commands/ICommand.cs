using MediatR;

namespace JackSite.Shared.CQRS.Commands;

/// <summary>
/// 命令接口（无返回值）
/// </summary>
public interface ICommand : IRequest
{
}

/// <summary>
/// 命令接口（有返回值）
/// </summary>
public interface ICommand<out TResult> : IRequest<TResult>
{
}