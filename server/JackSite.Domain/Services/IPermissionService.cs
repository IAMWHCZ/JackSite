using JackSite.Domain.Entities;

namespace JackSite.Domain.Services;

public interface IPermissionService
{
    Task<Permission> CreatePermissionAsync(string name, string code, string description, CancellationToken cancellationToken = default);
    Task<bool> UpdatePermissionAsync(long permissionId, string name, string code, string description, CancellationToken cancellationToken = default);
    Task<bool> DeletePermissionAsync(long permissionId, CancellationToken cancellationToken = default);
    Task<Permission?> GetPermissionByIdAsync(long permissionId, CancellationToken cancellationToken = default);
    Task<Permission?> GetPermissionByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetPermissionRolesAsync(long permissionId, CancellationToken cancellationToken = default);
}