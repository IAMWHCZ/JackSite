using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace JackSite.Identity.Server.Services
{
    public class ExternalUserStore(
        IExternalUserService externalUserService,
        IDistributedCache cache,
        IConfiguration configuration) : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // This implementation doesn't support creating users in the external system
            return IdentityResult.Failed(new IdentityError { 
                Description = "Creating users in the external system is not supported" 
            });
        }
        
        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // This implementation doesn't support deleting users in the external system
            return IdentityResult.Failed(new IdentityError { 
                Description = "Deleting users in the external system is not supported" 
            });
        }
        
        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            // Try to get from cache first
            var cacheKey = $"ExternalUser_{userId}";
            var cachedUser = await cache.GetStringAsync(cacheKey, cancellationToken);
            
            if (!string.IsNullOrEmpty(cachedUser))
            {
                return JsonSerializer.Deserialize<ApplicationUser>(cachedUser);
            }
            
            // Get from external system
            var externalUser = await externalUserService.GetUserByIdAsync(userId);
            if (externalUser == null)
            {
                return null;
            }
            
            var applicationUser = MapToApplicationUser(externalUser);
            
            // Cache the user
            await CacheUserAsync(applicationUser, cancellationToken);
            
            return applicationUser;
        }
        
        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var externalUser = await externalUserService.GetUserByUsernameAsync(normalizedUserName);
            if (externalUser == null)
            {
                return null;
            }
            
            var applicationUser = MapToApplicationUser(externalUser);
            
            // Cache the user
            await CacheUserAsync(applicationUser, cancellationToken);
            
            return applicationUser;
        }
        
        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }
        
        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }
        
        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }
        
        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }
        
        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }
        
        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // This implementation doesn't support updating users in the external system
            return Task.FromResult(IdentityResult.Failed(new IdentityError { 
                Description = "Updating users in the external system is not supported" 
            }));
        }
        
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            // This implementation doesn't store password hashes
            return Task.CompletedTask;
        }
        
        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // This implementation doesn't store password hashes
            return Task.FromResult<string>(null);
        }
        
        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // External users always have passwords in the external system
            return Task.FromResult(true);
        }
        
        public void Dispose()
        {
            // Nothing to dispose
        }
        
        private static ApplicationUser MapToApplicationUser(ExternalUserInfo externalUser)
        {
            return new ApplicationUser
            {
                Id = externalUser.Id,
                UserName = externalUser.Username,
                NormalizedUserName = externalUser.Username.ToUpper(),
                Email = externalUser.Email,
                NormalizedEmail = externalUser.Email.ToUpper(),
                EmailConfirmed = true,
                FirstName = externalUser.FirstName,
                LastName = externalUser.LastName,
                IsExternalUser = true,
                ExternalUserId = externalUser.Id,
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }
        
        private async Task CacheUserAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var cacheKey = $"ExternalUser_{user.Id}";
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = 
                    TimeSpan.FromMinutes(configuration.GetValue<int>("UserCache:ExpirationMinutes"))
            };
            
            await cache.SetStringAsync(
                cacheKey, 
                JsonSerializer.Serialize(user),
                cacheOptions,
                cancellationToken);
        }
    }
}