using System.ComponentModel;

namespace JackSite.User.WebAPI.Enums;

/// <summary>
/// 资源类型
/// </summary>
public enum ResourceType
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Description("菜单")]
    Menu = 0,
    
    /// <summary>
    /// 按钮
    /// </summary>
    [Description("按钮")]
    Button = 1,
    
    /// <summary>
    /// API接口
    /// </summary>
    [Description("API接口")]
    Api = 2,
    
    /// <summary>
    /// 数据表
    /// </summary>
    [Description("数据表")]
    Table = 3,
    
    /// <summary>
    /// 数据字段
    /// </summary>
    [Description("数据字段")]
    Field = 4,
    
    /// <summary>
    /// 文件
    /// </summary>
    [Description("文件")]
    File = 5,
    
    /// <summary>
    /// 其他资源
    /// </summary>
    [Description("其他资源")]
    Other = 99
}