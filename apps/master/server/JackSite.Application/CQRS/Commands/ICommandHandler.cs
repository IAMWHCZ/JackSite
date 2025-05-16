namespace JackSite.Application.CQRS.Commands;

/// <summary>
/// 命令处理器接口
/// </summary>
/// <typeparam name="TCommand">命令类型</typeparam>
/// <typeparam name="TResult">结果类型</typeparam>
public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>命令执行结果</returns>
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}



/// <summary>
/// 无返回值命令处理器接口
/// </summary>
/// <typeparam name="TCommand">命令类型</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task<Unit> Handle(TCommand command, CancellationToken cancellationToken);
}