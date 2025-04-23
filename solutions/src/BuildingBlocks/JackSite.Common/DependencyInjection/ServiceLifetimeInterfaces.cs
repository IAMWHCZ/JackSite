namespace JackSite.Common.DependencyInjection;

/// <summary>
/// 瞬时生命周期服务标记接口
/// </summary>
public interface ITransientService;

/// <summary>
/// 作用域生命周期服务标记接口
/// </summary>
public interface IScopedService;

/// <summary>
/// 单例生命周期服务标记接口
/// </summary>
public interface ISingletonService;