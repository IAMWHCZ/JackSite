using JackSite.Domain.Entities;

namespace JackSite.Domain.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserBasic>> GetRoleUsersAsync(long roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(long roleId, CancellationToken cancellationToken = default);
    Task AddPermissionToRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default);
    Task RemovePermissionFromRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default);
}