namespace JackSite.Authentication.Entities.Emails;

public class EmailAttachment : DraftableEntity
{
    [Description("存储对象的键")] public string ObjectKey { get; set; } = null!;

    [Description("附件描述")] public string? Description { get; set; }

    [Description("文件名称")] public string FileName { get; set; } = null!;

    [Description("文件大小（字节）")] public long FileSize { get; set; }

    [Description("MIME类型")] public string? ContentType { get; set; }

    [Description("是否为内联附件")] public bool IsInline { get; set; } = false;

    [Description("内容ID")] public string? ContentId { get; set; }

    [Description("文件扩展名")] public string? FileExtension { get; set; }

    [Description("上传时间")] public DateTimeOffset UploadTime { get; set; } = DateTimeOffset.UtcNow;

    [Description("存储位置类型")] public string StorageType { get; set; } = "local";

    [Description("邮件记录ID")] public Guid EmailRecordId { get; set; }

    [Description("关联的邮件基础信息")] public virtual EmailBasic EmailBasic { get; set; } = null!;
}