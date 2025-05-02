using System.ComponentModel;

namespace JackSite.User.WebAPI.Enums;

/// <summary>
/// 用户状态
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// 启用
    /// </summary>
    [Description("启用")]
    Enabled = 0,
    
    /// <summary>
    /// 禁用
    /// </summary>
    [Description("禁用")]
    Disabled = 1,
    
    /// <summary>
    /// 锁定
    /// </summary>
    [Description("锁定")]
    Locked = 2,
    
    /// <summary>
    /// 待验证
    /// </summary>
    [Description("待验证")]
    Pending = 3
}