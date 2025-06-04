namespace JackSite.Authentication.Entities.Users;

public class UserCredential : Entity
{
    public long UserId { get; private set; }
    public string Type { get; private set; } = null!; // password, otp, certificate等
    public string Value { get; private set; } = null!;
    public string? Salt { get; private set; }
    public int? HashIterations { get; private set; }
    public bool Temporary { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public virtual UserBasic User { get; private set; } = null!;
}