using System.ComponentModel;

namespace JackSite.Authentication.Entities.Emails;

/// <summary>
/// 邮件模板实体
/// </summary>
public class EmailTemplate : DraftableEntity
{
    [Description("模板名称")] public string Name { get; set; } = null!;

    [Description("模板描述")] public string? Description { get; set; }

    [Description("邮件主题")] public string Subject { get; set; } = null!;

    [Description("邮件正文")] public string Body { get; set; } = null!;

    [Description("是否HTML格式")] public bool IsHtml { get; set; } = true;

    [Description("参数定义")] public string? Parameters { get; set; }

    [Description("模板分类")] public string? Category { get; set; }

    [Description("模板版本")] public int Version { get; set; } = 1;

    [Description("是否为系统模板")] public bool IsSystem { get; set; } = false;

    [Description("上次使用时间")] public DateTimeOffset? LastUsedTime { get; set; }

    [Description("使用次数")] public int UsageCount { get; set; } = 0;
}