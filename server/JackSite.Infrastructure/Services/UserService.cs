using System.Security.Cryptography;
using System.Text;
using JackSite.Domain.Entities;
using JackSite.Domain.Services;

namespace JackSite.Infrastructure.Services;

public class UserService(
    IUserBasicRepository userRepository,
    IRoleRepository roleRepository,
    IBaseRepository<UserRole> userRoleRepository)
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

        var user = new UserBasic
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            Salt = salt,
            IsActive = true
        };

        return await userRepository.AddAsync(user, cancellationToken);
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

        // 检查用户是否已经拥有该角色
        var existingUserRole = await userRoleRepository.FindOneAsync(
            ur => ur.UserId == userId && ur.RoleId == roleId,
            cancellationToken);

        if (existingUserRole != null)
        {
            return true; // 用户已经拥有该角色
        }

        // 分配角色给用户
        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };

        await userRoleRepository.AddAsync(userRole, cancellationToken);
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