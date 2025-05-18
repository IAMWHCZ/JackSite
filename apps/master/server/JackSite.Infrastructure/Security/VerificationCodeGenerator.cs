using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace JackSite.Infrastructure.Security;

/// <summary>
/// 验证码生成器配置选项
/// </summary>
public class VerificationCodeOptions
{
    /// <summary>
    /// 验证码长度，默认为6位
    /// </summary>
    public int Length { get; set; } = 6;

    /// <summary>
    /// 验证码字符集，默认为数字
    /// </summary>
    public string CharacterSet { get; set; } = "0123456789";

    /// <summary>
    /// 验证码有效期（分钟），默认为30分钟
    /// </summary>
    public int ExpirationMinutes { get; set; } = 30;
}

/// <summary>
/// 验证码生成器
/// </summary>
/// <remarks>
/// 此类用于生成随机验证码，可用于邮件验证、短信验证等场景。
/// 支持配置验证码长度、字符集和有效期。
/// </remarks>
public class VerificationCodeGenerator
{
    private readonly VerificationCodeOptions _options;

    /// <summary>
    /// 初始化验证码生成器
    /// </summary>
    /// <param name="options">验证码生成器配置选项</param>
    public VerificationCodeGenerator(IOptions<VerificationCodeOptions>? options = null)
    {
        _options = options?.Value ?? new VerificationCodeOptions();
    }

    /// <summary>
    /// 生成随机验证码
    /// </summary>
    /// <returns>生成的验证码</returns>
    /// <remarks>
    /// 此方法使用加密安全的随机数生成器创建验证码，
    /// 确保生成的验证码具有足够的随机性和安全性。
    /// </remarks>
    public string Generate()
    {
        return Generate(_options.Length, _options.CharacterSet);
    }

    /// <summary>
    /// 生成指定长度和字符集的随机验证码
    /// </summary>
    /// <param name="length">验证码长度</param>
    /// <param name="characterSet">验证码字符集</param>
    /// <returns>生成的验证码</returns>
    /// <exception cref="ArgumentException">当长度小于1或字符集为空时抛出</exception>
    /// <remarks>
    /// 此方法允许自定义验证码的长度和字符集，
    /// 可以根据不同的安全需求生成不同复杂度的验证码。
    /// </remarks>
    public static string Generate(int length, string characterSet)
    {
        if (length < 1)
        {
            throw new ArgumentException("验证码长度必须大于0", nameof(length));
        }

        if (string.IsNullOrEmpty(characterSet))
        {
            throw new ArgumentException("字符集不能为空", nameof(characterSet));
        }

        var result = new char[length];
        var characterSetLength = characterSet.Length;

        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[length];

        // 使用加密安全的随机数生成器
        rng.GetBytes(randomBytes);

        // 将随机字节转换为字符
        for (var i = 0; i < length; i++)
        {
            var randomIndex = randomBytes[i] % characterSetLength;
            result[i] = characterSet[randomIndex];
        }

        return new string(result);
    }

    /// <summary>
    /// 生成仅包含数字的验证码
    /// </summary>
    /// <param name="length">验证码长度，默认为6位</param>
    /// <returns>生成的数字验证码</returns>
    /// <remarks>
    /// 此方法生成仅包含数字的验证码，适用于短信验证码等场景。
    /// </remarks>
    public static string GenerateNumeric(int length = 6)
    {
        return Generate(length, "0123456789");
    }

    /// <summary>
    /// 生成包含字母和数字的验证码
    /// </summary>
    /// <param name="length">验证码长度，默认为6位</param>
    /// <returns>生成的字母数字验证码</returns>
    /// <remarks>
    /// 此方法生成包含大小写字母和数字的验证码，提供更高的复杂度。
    /// </remarks>
    public static string GenerateAlphanumeric(int length = 6)
    {
        return Generate(length, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
    }

    /// <summary>
    /// 创建验证码结果，包含验证码和过期时间
    /// </summary>
    /// <returns>验证码结果</returns>
    /// <remarks>
    /// 此方法生成验证码并设置其过期时间，便于后续验证。
    /// </remarks>
    public VerificationCodeResult CreateCodeResult()
    {
        var code = Generate();
        var expirationTime = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);
        return new VerificationCodeResult(code, expirationTime);
    }
}

/// <summary>
/// 验证码结果
/// </summary>
/// <remarks>
/// 此类包含生成的验证码和其过期时间。
/// </remarks>
public class VerificationCodeResult
{
    /// <summary>
    /// 验证码
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// 过期时间（UTC）
    /// </summary>
    public DateTime ExpirationTime { get; }

    /// <summary>
    /// 初始化验证码结果
    /// </summary>
    /// <param name="code">验证码</param>
    /// <param name="expirationTime">过期时间</param>
    public VerificationCodeResult(string code, DateTime expirationTime)
    {
        Code = code;
        ExpirationTime = expirationTime;
    }

    /// <summary>
    /// 检查验证码是否已过期
    /// </summary>
    /// <returns>如果验证码已过期，则返回true；否则返回false</returns>
    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpirationTime;
    }

    /// <summary>
    /// 验证提供的代码是否匹配且未过期
    /// </summary>
    /// <param name="inputCode">用户输入的验证码</param>
    /// <returns>如果验证码匹配且未过期，则返回true；否则返回false</returns>
    public bool Validate(string inputCode)
    {
        if (IsExpired())
        {
            return false;
        }

        return string.Equals(Code, inputCode, StringComparison.Ordinal);
    }
}