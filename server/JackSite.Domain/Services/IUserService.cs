using JackSite.Domain.Entities;

namespace JackSite.Domain.Services;

public interface IUserService
{
    Task<UserBasic?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<UserBasic> RegisterAsync(string username, string email, string password, CancellationToken cancellationToken = default);
    Task<bool> AssignRoleToUserAsync(long userId, long roleId, CancellationToken cancellationToken = default);
    Task<bool> RemoveRoleFromUserAsync(long userId, long roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetUserPermissionsAsync(long userId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(long userId, string permissionCode, CancellationToken cancellationToken = default);
}