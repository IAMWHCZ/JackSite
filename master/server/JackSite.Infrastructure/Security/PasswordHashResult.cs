namespace JackSite.Infrastructure.Security;

/// <summary>
/// 密码哈希结果
/// </summary>
public class PasswordHashResult
{
    /// <summary>
    /// 哈希后的密码
    /// </summary>
    public string Hash { get; }
    
    /// <summary>
    /// 盐值
    /// </summary>
    public string Salt { get; }
    
    /// <summary>
    /// 迭代次数
    /// </summary>
    public int Iterations { get; }
    
    public PasswordHashResult(string hash, string salt, int iterations)
    {
        Hash = hash;
        Salt = salt;
        Iterations = iterations;
    }
}
