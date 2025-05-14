using System.Security.Cryptography;
using System.Text;
using JackSite.Domain.Entities;
using JackSite.Domain.Services;

namespace JackSite.Infrastructure.Services;

public class UserService(
    IUserBasicRepository userRepository,
    IRoleRepository roleRepository,
    IBaseRepository<UserRole,long> userRoleRepository,
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

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role == null)
        {
            return false;
        }

        // 使用领域行为添加角色
        user.AddRole(role);
        
        // 使用工作单元模式保存
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoveRoleFromUserAsync(long userId, long roleId,
        CancellationToken cancellationToken = default)
    {
        // 查找用户角色关系
        var userRole = await userRoleRepository.FindOneAsync(
            ur => ur.UserId == userId && ur.RoleId == roleId,
            cancellationToken);

        if (userRole == null)
        {
            return false; // 用户没有该角色
        }

        // 移除用户角色关系
        await userRoleRepository.DeleteAsync(userRole, cancellationToken);
        return true;
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await userRepository.GetUserRolesAsync(userId, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        return await userRepository.GetUserPermissionsAsync(userId, cancellationToken);
    }

    public async Task<bool> HasPermissionAsync(long userId, string permissionCode,
        CancellationToken cancellationToken = default)
    {
        return await userRepository.HasPermissionAsync(userId, permissionCode, cancellationToken);
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