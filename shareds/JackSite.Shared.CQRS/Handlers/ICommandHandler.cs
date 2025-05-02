using JackSite.Shared.CQRS.Commands;
using MediatR;

namespace JackSite.Shared.CQRS.Handlers;

/// <summary>
/// 命令处理器接口（无返回值）
/// </summary>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}

/// <summary>
/// 命令处理器接口（有返回值）
/// </summary>
public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
}