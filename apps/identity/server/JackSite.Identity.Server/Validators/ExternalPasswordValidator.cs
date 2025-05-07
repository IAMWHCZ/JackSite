using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace JackSite.Identity.Server.Services
{
    public class ExternalPasswordValidator : IPasswordValidator<ApplicationUser>
    {
        private readonly IExternalUserService _externalUserService;
        
        public ExternalPasswordValidator(IExternalUserService externalUserService)
        {
            _externalUserService = externalUserService;
        }
        
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            if (!user.IsExternalUser)
            {
                // For non-external users, let the default password validator handle it
                return IdentityResult.Success;
            }
            
            // For external users, validate against the external system
            var isValid = await _externalUserService.ValidateUserCredentialsAsync(user.UserName, password);
            
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