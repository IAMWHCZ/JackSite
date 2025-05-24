using JackSite.Domain.Base;

namespace JackSite.Domain.Services;

/// <summary>
/// 请求头参数服务接口
/// </summary>
public interface IRequestHeaderService
{
    /// <summary>
    /// 获取或设置基础请求头参数
    /// </summary>
    BaseHeaderParams HeaderParams { get; }
    
    /// <summary>
    /// 获取请求头中的参数值
    /// </summary>
    /// <param name="headerName">请求头名称</param>
    /// <returns>请求头值，如果不存在则返回null</returns>
    string? GetHeaderValue(string headerName);
    
    /// <summary>
    /// 获取请求头中的参数值并转换为指定类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="headerName">请求头名称</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>转换后的值，如果转换失败则返回默认值</returns>
    T GetHeaderValue<T>(string headerName, T defaultValue);
    
    /// <summary>
    /// 获取客户端IP地址
    /// </summary>
    /// <returns>客户端IP地址</returns>
    string? GetClientIpAddress();
    
    /// <summary>
    /// 获取用户代理信息
    /// </summary>
    /// <returns>用户代理字符串</returns>
    string? GetUserAgent();
}