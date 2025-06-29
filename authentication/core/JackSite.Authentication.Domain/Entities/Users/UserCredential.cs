using JackSite.Authentication.Enums.Users;

namespace JackSite.Authentication.Entities.Users;

public class UserCredential : Entity
{
    [Description("用户ID")]
    public Guid UserId { get; private set; }
    
    [Description("凭据类型")]
    public UserCredentialType Type { get; private set; } = UserCredentialType.Password; 
    
    [Description("凭据值（加密后）")]
    public string Value { get; private set; } = null!;
    
    [Description("加密盐值")]
    public string? Salt { get; private set; }
    
    [Description("哈希迭代次数")]
    public int? HashIterations { get; private set; }
    
    [Description("是否为临时凭据")]
    public bool Temporary { get; private set; }
    
    [Description("失败尝试次数")]
    public byte FailedAttempts { get; private set; } = 0; 
    
    [Description("创建时间")]
    public DateTimeOffset CreatedAt { get; private set; }
    
    [Description("关联用户")]
    public virtual UserBasic User { get; private set; } = null!;
}