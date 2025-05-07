using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackSite.Identity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class UserSyncController(
        IUserSyncService userSyncService,
        ILogger<UserSyncController> logger) : ControllerBase
    {
        [HttpPost("sync-user/{externalUserId}")]
        public async Task<IActionResult> SyncUser(string externalUserId)
        {
            logger.LogInformation("Manual sync requested for external user: {ExternalUserId}", externalUserId);
            
            await userSyncService.SyncExternalUserAsync(externalUserId);
            
            return Ok(new { message = "User sync initiated" });
        }
        
        [HttpPost("sync-all")]
        public async Task<IActionResult> SyncAllUsers()
        {
            logger.LogInformation("Manual sync requested for all external users");
            
            await userSyncService.SyncAllExternalUsersAsync();
            
            return Ok(new { message = "All users sync initiated" });
        }
    }
}