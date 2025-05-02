namespace JackSite.User.WebAPI.Enums;

/// <summary>
/// 数据权限范围类型
/// </summary>
public enum DataScopeType
{
    /// <summary>
    /// 全部数据
    /// </summary>
    [Description("全部数据")]
    All = 0,
    
    /// <summary>
    /// 仅本人数据
    /// </summary>
    [Description("仅本人数据")]
    Self = 1,
    
    /// <summary>
    /// 自定义数据
    /// </summary>
    [Description("自定义数据")]
    Custom = 2
}