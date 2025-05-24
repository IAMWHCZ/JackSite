namespace JackSite.Infrastructure.Security;

/// <summary>
/// 密码哈希服务接口
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// 创建密码哈希
    /// </summary>
    /// <param name="password">明文密码</param>
    /// <returns>密码哈希结果</returns>
    PasswordHashResult HashPassword(string password);
    
    /// <summary>
    /// 验证密码
    /// </summary>
    /// <param name="password">明文密码</param>
    /// <param name="hash">存储的哈希值</param>
    /// <param name="salt">存储的盐值</param>
    /// <param name="iterations">迭代次数</param>
    /// <returns>密码是否匹配</returns>
    bool VerifyPassword(string password, string hash, string salt, int iterations = 310000);
}