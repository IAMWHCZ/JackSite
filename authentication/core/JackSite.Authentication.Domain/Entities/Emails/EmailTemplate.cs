namespace JackSite.Authentication.Entities.Emails;

/// <summary>
/// 邮件模板实体
/// </summary>
public class EmailTemplate : DraftableEntity
{
    /// <summary>
    /// 模板名称
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// 模板描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 邮件主题
    /// </summary>
    public string Subject { get; set; } = null!;
    
    /// <summary>
    /// 邮件正文
    /// </summary>
    public string Body { get; set; } = null!;
    
    /// <summary>
    /// 是否HTML格式
    /// </summary>
    public bool IsHtml { get; set; } = true;
    
    /// <summary>
    /// 参数定义（JSON格式）
    /// </summary>
    public string? Parameters { get; set; }
    
    /// <summary>
    /// 模板分类
    /// </summary>
    public string? Category { get; set; }
    
    /// <summary>
    /// 模板版本
    /// </summary>
    public int Version { get; set; } = 1;
    
    /// <summary>
    /// 是否为系统模板
    /// </summary>
    public bool IsSystem { get; set; } = false;
    
    /// <summary>
    /// 上次使用时间
    /// </summary>
    public DateTime? LastUsedTime { get; set; }
    
    /// <summary>
    /// 使用次数
    /// </summary>
    public int UsageCount { get; set; } = 0;
}
