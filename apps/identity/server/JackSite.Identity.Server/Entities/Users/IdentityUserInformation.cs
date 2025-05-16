namespace JackSite.Identity.Server.Entities.Users;

public class IdentityUserInformation
{
    [Required]
    [MaxLength(50)]
    [Comment("用户ID")]
    public string UserId { get; set; } = string.Empty;
    
    [MaxLength(255)]
    [Comment("用户头像")]
    public string? Avatar { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Comment("用户名")]
    public string UserName { get; set; } = string.Empty;
    
    [MaxLength(100)]
    [EmailAddress]
    [Comment("电子邮箱")]
    public string? Email { get; set; }
    
    [MaxLength(20)]
    [Phone]
    [Comment("电话号码")]
    public string? PhoneNumber { get; set; }
}

