namespace JackSite.Domain.Services;

/// <summary>
/// 日志服务接口
/// </summary>
public interface ILogService
{
    /// <summary>
    /// 记录调试信息
    /// </summary>
    void Debug(string message, params object[] propertyValues);
    
    /// <summary>
    /// 记录普通信息
    /// </summary>
    void Information(string message, params object[] propertyValues);
    
    /// <summary>
    /// 记录警告信息
    /// </summary>
    void Warning(string message, params object[] propertyValues);
    
    /// <summary>
    /// 记录错误信息
    /// </summary>
    void Error(Exception? exception, string message, params object[] propertyValues);
    
    /// <summary>
    /// 记录错误信息（无异常）
    /// </summary>
    void Error(string message, params object[] propertyValues);
    
    /// <summary>
    /// 记录致命错误
    /// </summary>
    void Fatal(Exception? exception, string message, params object[] propertyValues);
    
    /// <summary>
    /// 记录致命错误（无异常）
    /// </summary>
    void Fatal(string message, params object[] propertyValues);
    
    /// <summary>
    /// 获取指定类型的日志记录器
    /// </summary>
    ILogService ForContext<T>();
    
    /// <summary>
    /// 获取指定类型的日志记录器
    /// </summary>
    ILogService ForContext(Type type);
    
    /// <summary>
    /// 获取指定上下文的日志记录器
    /// </summary>
    ILogService ForContext(string propertyName, object? value, bool destructureObjects = false);
}