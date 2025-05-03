namespace JackSite.Domain.Entities;

public class UserRole : BaseEntity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    
    // 导航属性
    public virtual UserBasic UserBasic { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}