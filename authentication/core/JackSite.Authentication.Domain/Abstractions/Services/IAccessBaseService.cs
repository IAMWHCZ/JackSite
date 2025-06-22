using JackSite.Shared.Models;

namespace JackSite.Authentication.Abstractions.Services;

public interface IAccessBaseService
{

    /// <summary>
    /// 获取指定请求头的值
    /// </summary>
    /// <param name="headerName">请求头名称</param>
    /// <returns>请求头值，如不存在则返回 null</returns>
    string? GetHeaderValue(string headerName);
    
    /// <summary>
    /// 获取当前请求的用户表单信息
    /// </summary>
    /// <returns>用户表单信息</returns>
    HttpFromBase GetCurrentFormBase();
}