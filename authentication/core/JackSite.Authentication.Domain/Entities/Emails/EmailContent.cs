namespace JackSite.Authentication.Entities.Emails;

public class EmailContent : DraftableEntity
{
    /// <summary>
    /// 邮件内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 是否HTML格式
    /// </summary>
    public bool IsHtml { get; set; } = true;
    
    /// <summary>
    /// 邮件主题
    /// </summary>
    public string? Subject { get; set; }
    
    /// <summary>
    /// 纯文本内容（用于搜索和展示）
    /// </summary>
    public string? PlainTextContent { get; set; }
    
    /// <summary>
    /// 收件人（以分号分隔）
    /// </summary>
    public string? Recipient { get; set; }
    
    /// <summary>
    /// 抄送人（以分号分隔）
    /// </summary>
    public string? CC { get; set; }
    
    /// <summary>
    /// 密送人（以分号分隔）
    /// </summary>
    public string? BCC { get; set; }
    
    /// <summary>
    /// 模板ID（如果使用模板）
    /// </summary>
    public string? TemplateId { get; set; }
    
    /// <summary>
    /// 模板参数（JSON格式）
    /// </summary>
    public string? TemplateParameters { get; set; }
    
    /// <summary>
    /// 预览文本（显示在邮件列表中的摘要）
    /// </summary>
    public string? PreviewText { get; set; }

    public long EmailId { get; set; }

    public virtual EmailBasic EmailBasic { get; set; } = null!;
}
