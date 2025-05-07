using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace JackSite.Identity.Server.Services
{
    
    public class UserService(
        UserManager<ApplicationUser> userManager,
        IDistributedCache cache,
        IConfiguration configuration,
        IExternalUserService externalUserService) : IUserService
    {
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            // Try to get from cache first
            var cacheKey = $"User_{userId}";
            var cachedUser = await cache.GetStringAsync(cacheKey);
            
            if (!string.IsNullOrEmpty(cachedUser))
            {
                return JsonSerializer.Deserialize<ApplicationUser>(cachedUser)!;
            }
            
            // Get from database or external system
            var user = await userManager.FindByIdAsync(userId);
            
            if (user != null)
            {
                // Cache the user
                await CacheUserAsync(user);
            }
            
            return user!;
        }
        
        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            
            if (user != null)
            {
                await CacheUserAsync(user);
            }
            
            return user!;
        }
        
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            
            if (user != null)
            {
                await CacheUserAsync(user);
            }
            
            return user!;
        }
        
        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            
            if (user == null)
            {
                return false;
            }
            
            if (user.IsExternalUser)
            {
                // For external users, validate against the external system
                return await externalUserService.ValidateUserCredentialsAsync(username, password);
            }
            
            // For local users, use the UserManager
            return await userManager.CheckPasswordAsync(user, password);
        }
        
        private async Task CacheUserAsync(ApplicationUser user)
        {
            var cacheKey = $"User_{user.Id}";
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = 
                    TimeSpan.FromMinutes(configuration.GetValue<int>("UserCache:ExpirationMinutes"))
            };
            
            await cache.SetStringAsync(
                cacheKey, 
                JsonSerializer.Serialize(user),
                cacheOptions);
        }
    }
}