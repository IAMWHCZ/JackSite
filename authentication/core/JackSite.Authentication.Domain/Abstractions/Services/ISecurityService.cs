namespace JackSite.Authentication.Abstractions.Services;

public interface ISecurityService
{
    /// <summary>
    /// 生成随机密码
    /// </summary>
    /// <param name="length">密码长度，默认12位</param>
    /// <returns>随机密码</returns>
    string GenerateRandomPassword(int length = 12);

    /// <summary>
    /// 生成盐值
    /// </summary>
    /// <returns>Base64编码的盐值</returns>
    string GenerateSalt();

    /// <summary>
    /// 使用盐值哈希密码
    /// </summary>
    /// <param name="password">原始密码</param>
    /// <param name="salt">盐值</param>
    /// <returns>哈希后的密码</returns>
    string HashPasswordWithSalt(string password, string salt);

    /// <summary>
    /// 验证密码
    /// </summary>
    /// <param name="password">原始密码</param>
    /// <param name="salt">盐值</param>
    /// <param name="hashedPassword">存储的哈希密码</param>
    /// <returns>密码是否匹配</returns>
    bool VerifyPassword(string password, string salt, string hashedPassword);
}