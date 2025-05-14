namespace JackSite.Infrastructure.Repositories;

public class UserBasicRepository(ApplicationDbContext dbContext)
    : BaseRepository<UserBasic,long>(dbContext), IUserBasicRepository
{
    

    public async Task<UserBasic?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<UserBasic>()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<UserBasic?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<UserBasic>()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Role>()
            .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Permission>()
            .Where(p => p.RolePermissions.Any(rp => 
                rp.Role.UserRoles.Any(ur => ur.UserId == userId)))
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasPermissionAsync(long userId, string permissionCode, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Permission>()
            .AnyAsync(p => p.Code == permissionCode && 
                p.RolePermissions.Any(rp => 
                    rp.Role.UserRoles.Any(ur => ur.UserId == userId)), 
                cancellationToken);
    }
}