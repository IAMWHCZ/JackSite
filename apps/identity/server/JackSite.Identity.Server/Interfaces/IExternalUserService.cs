using JackSite.Identity.Server.Models;

namespace JackSite.Identity.Server.Interfaces
{
    public interface IExternalUserService
    {
        Task<ExternalUserInfo> GetUserByIdAsync(string externalUserId);
        Task<ExternalUserInfo> GetUserByUsernameAsync(string username);
        Task<ExternalUserInfo> GetUserByEmailAsync(string email);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
    }
}