using JackSite.Domain.Entities;

namespace JackSite.Domain.Services;

public interface IUserService
{
    Task<UserBasic?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<UserBasic> RegisterAsync(string username, string email, string password, CancellationToken cancellationToken = default);
}