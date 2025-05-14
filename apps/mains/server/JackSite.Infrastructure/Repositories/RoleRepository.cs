namespace JackSite.Infrastructure.Repositories;

public class RoleRepository(ApplicationDbContext dbContext) : BaseRepository<Role,long>(dbContext), IRoleRepository
{
    

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<UserBasic>> GetRoleUsersAsync(long roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<UserBasic>()
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(long roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Permission>()
            .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId))
            .ToListAsync(cancellationToken);
    }

    public async Task AddPermissionToRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Set<RolePermission>()
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);
        
        if (!exists)
        {
            var rolePermission = new RolePermission(roleId,permissionId);
            
            await dbContext.Set<RolePermission>().AddAsync(rolePermission, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RemovePermissionFromRoleAsync(long roleId, long permissionId, CancellationToken cancellationToken = default)
    {
        var rolePermission = await dbContext.Set<RolePermission>()
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);
        
        if (rolePermission != null)
        {
            dbContext.Set<RolePermission>().Remove(rolePermission);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}