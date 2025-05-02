using System.ComponentModel;

namespace JackSite.User.WebAPI.Enums;

/// <summary>
/// 权限类型
/// </summary>
public enum PermissionType
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    [Description("菜单权限")]
    Menu = 0,
    
    /// <summary>
    /// 功能权限
    /// </summary>
    [Description("功能权限")]
    Function = 1,
    
    /// <summary>
    /// 数据权限
    /// </summary>
    [Description("数据权限")]
    Data = 2,
    
    /// <summary>
    /// API权限
    /// </summary>
    [Description("API权限")]
    Api = 3,
    
    /// <summary>
    /// 字段权限
    /// </summary>
    [Description("字段权限")]
    Field = 4,
    
    /// <summary>
    /// 其他权限
    /// </summary>
    [Description("其他权限")]
    Other = 99
}