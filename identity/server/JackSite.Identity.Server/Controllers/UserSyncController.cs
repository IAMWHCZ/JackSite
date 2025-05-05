using JackSite.Identity.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackSite.Identity.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class UserSyncController : ControllerBase
    {
        private readonly IUserSyncService _userSyncService;
        private readonly ILogger<UserSyncController> _logger;
        
        public UserSyncController(
            IUserSyncService userSyncService,
            ILogger<UserSyncController> logger)
        {
            _userSyncService = userSyncService;
            _logger = logger;
        }
        
        [HttpPost("sync-user/{externalUserId}")]
        public async Task<IActionResult> SyncUser(string externalUserId)
        {
            _logger.LogInformation("Manual sync requested for external user: {ExternalUserId}", externalUserId);
            
            await _userSyncService.SyncExternalUserAsync(externalUserId);
            
            return Ok(new { message = "User sync initiated" });
        }
        
        [HttpPost("sync-all")]
        public async Task<IActionResult> SyncAllUsers()
        {
            _logger.LogInformation("Manual sync requested for all external users");
            
            await _userSyncService.SyncAllExternalUsersAsync();
            
            return Ok(new { message = "All users sync initiated" });
        }
    }
}