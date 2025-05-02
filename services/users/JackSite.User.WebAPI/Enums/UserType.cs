using System.ComponentModel;

namespace JackSite.User.WebAPI.Enums;

/// <summary>
/// 用户类型
/// </summary>
public enum UserType
{
    /// <summary>
    /// 本地用户
    /// </summary>
    [Description("本地用户")]
    Local = 0,
    
    /// <summary>
    /// 第三方用户
    /// </summary>
    [Description("第三方用户")]
    ThirdParty = 1,
    
    /// <summary>
    /// 临时用户
    /// </summary>
    [Description("临时用户")]
    Temporary = 2,
    
    /// <summary>
    /// 系统用户
    /// </summary>
    [Description("系统用户")]
    System = 3
}