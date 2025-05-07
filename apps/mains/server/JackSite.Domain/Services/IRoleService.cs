using JackSite.Domain.Entities;

namespace JackSite.Domain.Services;

public interface IRoleService
{
    Task<Role> CreateRoleAsync(string name, string description, CancellationToken cancellationToken = default);
    Task<bool> UpdateRoleAsync(long roleId, string name, string description, CancellationToken cancellationToken = default);
    Task<bool> DeleteRoleAsync(long roleId, CancellationToken cancellationToken = default);
    Task<Role?> GetRoleByIdAsync(long roleId, CancellationToken cancellationToken = default);
    Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<bool> AddPermissionToRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default);
    Task<bool> RemovePermissionFromRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(long roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserBasic>> GetRoleUsersAsync(long roleId, CancellationToken cancellationToken = default);
}