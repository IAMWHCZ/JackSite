using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace JackSite.Identity.Server.Services
{
    public class ExternalPasswordValidator(IExternalUserService externalUserService) : IPasswordValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            if (!user.IsExternalUser)
            {
                // For non-external users, let the default password validator handle it
                return IdentityResult.Success;
            }
            
            // For external users, validate against the external system
            var isValid = await externalUserService.ValidateUserCredentialsAsync(user.UserName, password);
            
            if (isValid)
            {
                return IdentityResult.Success;
            }
            
            return IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidExternalPassword",
                Description = "The provided password is incorrect."
            });
        }
    }
}