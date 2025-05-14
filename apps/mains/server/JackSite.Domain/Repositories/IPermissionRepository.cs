using JackSite.Domain.Entities;

namespace JackSite.Domain.Repositories;

public interface IPermissionRepository : IBaseRepository<Permission,  long>
{
    Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetPermissionRolesAsync(long permissionId, CancellationToken cancellationToken = default);
}