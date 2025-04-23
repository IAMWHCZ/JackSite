namespace JackSite.Notification.Server.Entities;

public class EmailAttachment : EntityBase
{
    [Required]
    public SnowflakeId EmailId { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string FilePath { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string ContentType { get; set; } = null!;

    [Required]
    public long FileSize { get; set; }

    [Required]
    public DateTime UploadedAt { get; set; }
    
    // 导航属性
    [ForeignKey("email_id")]
    public Email Email { get; set; } = null!;
}