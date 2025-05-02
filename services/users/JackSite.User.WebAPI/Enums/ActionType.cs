using System.ComponentModel;

namespace JackSite.User.WebAPI.Enums;

/// <summary>
/// 操作类型
/// </summary>
public enum ActionType
{
    /// <summary>
    /// 查看
    /// </summary>
    [Description("查看")]
    View = 0,
    
    /// <summary>
    /// 新增
    /// </summary>
    [Description("新增")]
    Create = 1,
    
    /// <summary>
    /// 修改
    /// </summary>
    [Description("修改")]
    Update = 2,
    
    /// <summary>
    /// 删除
    /// </summary>
    [Description("删除")]
    Delete = 3,
    
    /// <summary>
    /// 导入
    /// </summary>
    [Description("导入")]
    Import = 4,
    
    /// <summary>
    /// 导出
    /// </summary>
    [Description("导出")]
    Export = 5,
    
    /// <summary>
    /// 审核
    /// </summary>
    [Description("审核")]
    Audit = 6,
    
    /// <summary>
    /// 执行
    /// </summary>
    [Description("执行")]
    Execute = 7,
    
    /// <summary>
    /// 打印
    /// </summary>
    [Description("打印")]
    Print = 8,
    
    /// <summary>
    /// 下载
    /// </summary>
    [Description("下载")]
    Download = 9,
    
    /// <summary>
    /// 上传
    /// </summary>
    [Description("上传")]
    Upload = 10,
    
    /// <summary>
    /// 其他操作
    /// </summary>
    [Description("其他操作")]
    Other = 99
}