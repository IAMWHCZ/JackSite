namespace JackSite.Authentication.Entities.Users;

public class UserAttribute : Entity
{
    [Description("用户ID")]
    public Guid UserId { get; private set; }

    [Description("属性名称")]
    public string Name { get; private set; } = null!;

    [Description("属性值")]
    public string Value { get; private set; } = null!;

    [Description("关联用户")]
    public virtual UserBasic User { get; private set; } = null!;
}