using JackSite.Identity.Server.Data;
using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Services
{    
    public class UserSyncService(
        IExternalUserService externalUserService,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext,
        ILogger<UserSyncService> logger) : IUserSyncService
    {
        public async Task SyncExternalUserAsync(string externalUserId)
        {
            try
            {
                var externalUser = await externalUserService.GetUserByIdAsync(externalUserId);
                if (externalUser == null)
                {
                    logger.LogWarning("External user not found: {ExternalUserId}", externalUserId);
                    return;
                }
                
                // Check if user already exists in our system
                var user = await userManager.FindByIdAsync(externalUser.Id.ToString());
                
                if (user == null)
                {
                    // Create new user
                    user = new ApplicationUser
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
                    
                    var result = await userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        logger.LogError("Failed to create user from external system: {Errors}", 
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                        return;
                    }
                    
                    logger.LogInformation("Created new user from external system: {UserId}", user.Id);
                }
                else
                {
                    // Update existing user
                    user.UserName = externalUser.Username;
                    user.NormalizedUserName = externalUser.Username.ToUpper();
                    user.Email = externalUser.Email;
                    user.NormalizedEmail = externalUser.Email.ToUpper();
                    user.FirstName = externalUser.FirstName;
                    user.LastName = externalUser.LastName;
                    
                    var result = await userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        logger.LogError("Failed to update user from external system: {Errors}", 
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                        return;
                    }
                    
                    logger.LogInformation("Updated user from external system: {UserId}", user.Id);
                }
                
                // Sync roles
                var currentRoles = await userManager.GetRolesAsync(user);
                var rolesToRemove = currentRoles.Except(externalUser.Roles).ToList();
                var rolesToAdd = externalUser.Roles.Except(currentRoles).ToList();
                
                if (rolesToRemove.Any())
                {
                    var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                    {
                        logger.LogError("Failed to remove roles for user {UserId}: {Errors}", 
                            user.Id, string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                    }
                }
                
                if (rolesToAdd.Any())
                {
                    var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!addResult.Succeeded)
                    {
                        logger.LogError("Failed to add roles for user {UserId}: {Errors}", 
                            user.Id, string.Join(", ", addResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error syncing external user: {ExternalUserId}", externalUserId);
            }
        }
        
        public async Task SyncAllExternalUsersAsync()
        {
            try
            {
                // This method would typically call an API endpoint that returns all users
                // For demonstration purposes, we'll just sync users we already know about
                
                var externalUsers = await dbContext.Users
                    .Where(u => u.IsExternalUser)
                    .Select(u => u.ExternalUserId)
                    .ToListAsync();
                
                logger.LogInformation("Starting sync of {Count} external users", externalUsers.Count);
                
                foreach (var externalUserId in externalUsers)
                {
                    await SyncExternalUserAsync(externalUserId.ToString());
                }
                
                logger.LogInformation("Completed sync of {Count} external users", externalUsers.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error syncing all external users");
            }
        }
    }
}