using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

namespace JackSite.Infrastructure.Security;

/// <summary>
/// 密码哈希服务实现
/// </summary>
public class PasswordHasher(IOptions<PasswordHasherOptions>? options = null) : IPasswordHasher
{
    private readonly PasswordHasherOptions _options = options?.Value ?? new PasswordHasherOptions();

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
        var salt = new byte[_options.SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        
        // 使用PBKDF2算法生成哈希
        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: _options.Iterations,
            numBytesRequested: _options.HashSize);
        
        // 转换为Base64字符串
        var saltString = Convert.ToBase64String(salt);
        var hashString = Convert.ToBase64String(hash);
        
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
            var saltBytes = Convert.FromBase64String(salt);
            
            // 使用相同的参数重新计算哈希
            var hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: _options.HashSize);
            
            // 将计算出的哈希转换为Base64字符串
            var computedHash = Convert.ToBase64String(hashBytes);
            
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