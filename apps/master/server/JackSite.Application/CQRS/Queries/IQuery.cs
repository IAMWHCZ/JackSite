namespace JackSite.Application.CQRS.Queries;

/// <summary>
/// 查询接口
/// </summary>
/// <typeparam name="TResult">结果类型</typeparam>
public interface IQuery<out TResult>;
