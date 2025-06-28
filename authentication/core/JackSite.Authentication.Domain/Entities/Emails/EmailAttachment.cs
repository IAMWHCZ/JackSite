namespace JackSite.Authentication.Entities.Emails;

public class EmailAttachment : DraftableEntity
{
    /// <summary>
    /// 存储对象的键（如S3 Key或文件路径）
    /// </summary>
    public string ObjectKey { get; set; } = null!;

    /// <summary>
    /// 附件描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = null!;
    
    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    public long FileSize { get; set; }
    
    /// <summary>
    /// MIME类型
    /// </summary>
    public string? ContentType { get; set; }
    
    /// <summary>
    /// 内联附件（true）或常规附件（false）
    /// </summary>
    public bool IsInline { get; set; } = false;
    
    /// <summary>
    /// 内容ID（用于HTML中引用内联附件）
    /// </summary>
    public string? ContentId { get; set; }
    
    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; }
    
    /// <summary>
    /// 上传时间
    /// </summary>
    public DateTime UploadTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 存储位置（如：local, s3, azure等）
    /// </summary>
    public string StorageType { get; set; } = "local";

    public Guid EmailRecordId { get; set; }

    public virtual EmailBasic EmailBasic { get; set; } = null!;
}
