using JackSite.Domain.Entities;

namespace JackSite.Infrastructure.Repositories;

public class PermissionRepository(ApplicationDbContext dbContext)
    : BaseRepository<Permission,long>(dbContext), IPermissionRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Permission>()
            .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetPermissionRolesAsync(long permissionId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
            .Where(r => r.RolePermissions.Any(rp => rp.PermissionId == permissionId))
            .ToListAsync(cancellationToken);
    }
}