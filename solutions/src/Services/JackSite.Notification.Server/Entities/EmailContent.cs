using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JackSite.Notification.Server.Entities;

[Table("EmailContents")]
public class EmailContent : EntityBase
{
    [Required]
    public long EmailId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Body { get; set; } = null!;
    
    // 导航属性
    [ForeignKey("email_id")]
    public Email Email { get; set; } = null!;
}