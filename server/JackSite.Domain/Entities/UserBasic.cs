namespace JackSite.Domain.Entities;

public class UserBasic : BaseEntity, ISoftDeletable
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    
    // 导航属性
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}