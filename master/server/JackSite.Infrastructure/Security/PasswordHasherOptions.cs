namespace JackSite.Infrastructure.Security;

/// <summary>
/// 密码哈希配置选项
/// </summary>
public class PasswordHasherOptions
{
    /// <summary>
    /// PBKDF2迭代次数，默认为310000
    /// 注意：随着硬件性能提升，此值应定期增加
    /// </summary>
    public int Iterations { get; set; } = 310000;
    
    /// <summary>
    /// 盐值长度（字节数）
    /// </summary>
    public int SaltSize { get; set; } = 16;
    
    /// <summary>
    /// 哈希长度（字节数）
    /// </summary>
    public int HashSize { get; set; } = 32;
}
