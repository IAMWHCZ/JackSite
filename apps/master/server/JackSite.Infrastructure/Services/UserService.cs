using System.Security.Cryptography;
using System.Text;
using JackSite.Domain.Entities;
using JackSite.Domain.Services;

namespace JackSite.Infrastructure.Services;

public class UserService(
    IUserBasicRepository userRepository,
    IUnitOfWork unitOfWork)
    : IUserService
{
    public async Task<UserBasic?> AuthenticateAsync(string username, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUsernameAsync(username, cancellationToken);

        if (user is not { IsActive: true })
        {
            return null;
        }

        var passwordHash = HashPassword(password, user.Salt);

        return user.PasswordHash == passwordHash ? user : null;
    }

    public async Task<UserBasic> RegisterAsync(string username, string email, string password,
        CancellationToken cancellationToken = default)
    {
        // 检查用户名和邮箱是否已存在
        var existingUser = await userRepository.GetByUsernameAsync(username, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Username '{username}' is already taken.");
        }

        existingUser = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Email '{email}' is already registered.");
        }

        // 创建新用户
        var salt = GenerateSalt();
        var passwordHash = HashPassword(password, salt);

        // 使用领域构造函数创建用户实体
        var user = new UserBasic(username, email, passwordHash, salt);
        
        // 使用工作单元模式保存
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return user;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> AssignRoleToUserAsync(long userId, long roleId,
        CancellationToken cancellationToken = default)
    {
        // 检查用户和角色是否存在
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return false;
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    #region 辅助方法

    private static string GenerateSalt()
    {
        var saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    private static string HashPassword(string password, string salt)
    {
        var passwordWithSalt = password + salt;
        var passwordBytes = Encoding.UTF8.GetBytes(passwordWithSalt);
        var hashBytes = SHA256.HashData(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

    #endregion
}