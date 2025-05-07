using JackSite.Identity.Server.Models;

namespace JackSite.Identity.Server.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
    }
}