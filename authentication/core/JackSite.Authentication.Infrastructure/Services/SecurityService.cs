using System.Security.Cryptography;

namespace JackSite.Authentication.Infrastructure.Services;

public class SecurityService : ISecurityService
{
    public string GenerateRandomPassword(int length = 12)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[32];
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    public string HashPasswordWithSalt(string password, string salt)
    {
        var saltedPassword = password + salt;
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string salt, string hashedPassword)
    {
        var hashToVerify = HashPasswordWithSalt(password, salt);
        return hashToVerify.Equals(hashedPassword, StringComparison.Ordinal);
    }
}