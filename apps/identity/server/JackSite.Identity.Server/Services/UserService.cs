using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace JackSite.Identity.Server.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
    }
    
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IExternalUserService _externalUserService;
        
        public UserService(
            UserManager<ApplicationUser> userManager,
            IDistributedCache cache,
            IConfiguration configuration,
            IExternalUserService externalUserService)
        {
            _userManager = userManager;
            _cache = cache;
            _configuration = configuration;
            _externalUserService = externalUserService;
        }
        
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            // Try to get from cache first
            var cacheKey = $"User_{userId}";
            var cachedUser = await _cache.GetStringAsync(cacheKey);
            
            if (!string.IsNullOrEmpty(cachedUser))
            {
                return JsonSerializer.Deserialize<ApplicationUser>(cachedUser);
            }
            
            // Get from database or external system
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user != null)
            {
                // Cache the user
                await CacheUserAsync(user);
            }
            
            return user;
        }
        
        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            
            if (user != null)
            {
                await CacheUserAsync(user);
            }
            
            return user;
        }
        
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user != null)
            {
                await CacheUserAsync(user);
            }
            
            return user;
        }
        
        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            
            if (user == null)
            {
                return false;
            }
            
            if (user.IsExternalUser)
            {
                // For external users, validate against the external system
                return await _externalUserService.ValidateUserCredentialsAsync(username, password);
            }
            
            // For local users, use the UserManager
            return await _userManager.CheckPasswordAsync(user, password);
        }
        
        private async Task CacheUserAsync(ApplicationUser user)
        {
            var cacheKey = $"User_{user.Id}";
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = 
                    TimeSpan.FromMinutes(_configuration.GetValue<int>("UserCache:ExpirationMinutes"))
            };
            
            await _cache.SetStringAsync(
                cacheKey, 
                JsonSerializer.Serialize(user),
                cacheOptions);
        }
    }
}