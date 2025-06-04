namespace JackSite.Authentication.Entities.Users;

public class UserAttribute : Entity
{
    public long UserId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Value { get; private set; } = null!;
    
    public virtual UserBasic User { get; private set; } = null!;
}