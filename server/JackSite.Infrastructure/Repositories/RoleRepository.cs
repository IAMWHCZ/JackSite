using JackSite.Domain.Entities;

namespace JackSite.Infrastructure.Repositories;

public class RoleRepository(ApplicationDbContext dbContext) : BaseRepository<Role>(dbContext), IRoleRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<UserBasic>> GetRoleUsersAsync(long roleId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UserBasic>()
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(long roleId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Permission>()
            .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId))
            .ToListAsync(cancellationToken);
    }

    public async Task AddPermissionToRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Set<RolePermission>()
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);
        
        if (!exists)
        {
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };
            
            await _dbContext.Set<RolePermission>().AddAsync(rolePermission, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RemovePermissionFromRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default)
    {
        var rolePermission = await _dbContext.Set<RolePermission>()
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);
        
        if (rolePermission != null)
        {
            _dbContext.Set<RolePermission>().Remove(rolePermission);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}