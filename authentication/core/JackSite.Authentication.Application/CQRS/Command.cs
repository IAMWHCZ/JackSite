namespace JackSite.Authentication.Application.CQRS;

/// <summary>
/// 命令接口 - 不返回结果
/// </summary>
public interface ICommand : IRequest<Unit>
{
    DateTime Timestamp { get; }
}

/// <summary>
/// 命令接口 - 返回结果
/// </summary>
/// <typeparam name="TResult">命令执行后返回的结果类型</typeparam>
public interface ICommand<out TResult> : IRequest<TResult>
{
    DateTime Timestamp { get; }
}

/// <summary>
/// 命令处理器接口 - 不返回结果
/// </summary>
/// <typeparam name="TCommand">要处理的命令类型</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand,Unit>
    where TCommand : ICommand;

/// <summary>
/// 命令处理器接口 - 返回结果
/// </summary>
/// <typeparam name="TCommand">要处理的命令类型</typeparam>
/// <typeparam name="TResult">命令执行后返回的结果类型</typeparam>
public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>;

/// <summary>
/// 命令基础实现 - 不返回结果
/// </summary>
public record CommandBase : ICommand
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

/// <summary>
/// 命令基础实现 - 返回结果
/// </summary>
/// <typeparam name="TResult">命令执行后返回的结果类型</typeparam>
public record CommandBase<TResult> : ICommand<TResult>
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

