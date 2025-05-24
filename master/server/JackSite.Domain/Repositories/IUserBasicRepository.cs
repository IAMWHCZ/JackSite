using JackSite.Domain.Entities;

namespace JackSite.Domain.Repositories;

public interface IUserBasicRepository : IBaseRepository<UserBasic,long>
{
    Task<UserBasic?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<UserBasic?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}