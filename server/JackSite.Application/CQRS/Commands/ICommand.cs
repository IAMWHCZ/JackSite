namespace JackSite.Application.CQRS.Commands;

/// <summary>
/// 命令接口标记
/// </summary>
public interface ICommand;

/// <summary>
/// 带返回结果的命令接口
/// </summary>
/// <typeparam name="TResult">结果类型</typeparam>
public interface ICommand<out TResult> : ICommand;