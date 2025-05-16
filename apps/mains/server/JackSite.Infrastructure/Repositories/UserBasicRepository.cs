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
}