using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

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

/// <summary>
/// 密码哈希服务实现
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasherOptions _options;
    
    public PasswordHasher(IOptions<PasswordHasherOptions>? options = null)
    {
        _options = options?.Value ?? new PasswordHasherOptions();
    }
    
    /// <summary>
    /// 创建密码哈希
    /// </summary>
    public PasswordHashResult HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("密码不能为空", nameof(password));
        }
        
        // 生成随机盐值
        byte[] salt = new byte[_options.SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        
        // 使用PBKDF2算法生成哈希
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: _options.Iterations,
            numBytesRequested: _options.HashSize);
        
        // 转换为Base64字符串
        string saltString = Convert.ToBase64String(salt);
        string hashString = Convert.ToBase64String(hash);
        
        return new PasswordHashResult(hashString, saltString, _options.Iterations);
    }
    
    /// <summary>
    /// 验证密码
    /// </summary>
    public bool VerifyPassword(string password, string hash, string salt, int iterations = 310000)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }
        
        if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(salt))
        {
            return false;
        }
        
        try
        {
            // 将盐值从Base64转换为字节数组
            byte[] saltBytes = Convert.FromBase64String(salt);
            
            // 使用相同的参数重新计算哈希
            byte[] hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: _options.HashSize);
            
            // 将计算出的哈希转换为Base64字符串
            string computedHash = Convert.ToBase64String(hashBytes);
            
            // 比较计算出的哈希与存储的哈希
            return hash == computedHash;
        }
        catch
        {
            // 如果发生任何异常（例如，无效的Base64字符串），返回false
            return false;
        }
    }
}