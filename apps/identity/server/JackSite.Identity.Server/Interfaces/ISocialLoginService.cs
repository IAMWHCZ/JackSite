using JackSite.Identity.Server.Enums;
using JackSite.Identity.Server.Models;

namespace JackSite.Identity.Server.Interfaces
{
        public interface ISocialLoginService
    {
        Task<ApplicationUser> ProcessExternalLoginAsync(SocialProvider  provider, string code, string redirectUri);
        string GetAuthorizationUrl(string provider, string redirectUri, string state);
    }
}