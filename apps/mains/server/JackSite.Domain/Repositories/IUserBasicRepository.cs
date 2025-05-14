using JackSite.Domain.Entities;

namespace JackSite.Domain.Repositories;

public interface IUserBasicRepository : IBaseRepository<UserBasic,long>
{
    Task<UserBasic?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<UserBasic?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetUserPermissionsAsync(long userId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(long userId, string permissionCode, CancellationToken cancellationToken = default);
}