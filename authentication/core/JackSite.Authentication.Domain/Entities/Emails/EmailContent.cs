namespace JackSite.Authentication.Entities.Emails;

public class EmailContent : DraftableEntity
{
    [Description("邮件内容")] public string? Content { get; set; }

    [Description("是否HTML格式")] public bool IsHtml { get; set; } = true;

    [Description("邮件主题")] public string? Subject { get; set; }

    [Description("纯文本内容")] public string? PlainTextContent { get; set; }

    [Description("收件人")] public string? Recipient { get; set; }

    [Description("抄送人")] public string? CC { get; set; }

    [Description("密送人")] public string? BCC { get; set; }

    [Description("模板ID")] public string? TemplateId { get; set; }

    [Description("模板参数")] public string? TemplateParameters { get; set; }

    [Description("预览文本")] public string? PreviewText { get; set; }

    [Description("邮件ID")]
    public Guid EmailId { get; set; }

    [Description("关联的邮件基础信息")]
    public virtual EmailBasic EmailBasic { get; set; } = null!;
}