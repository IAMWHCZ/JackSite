using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JackSite.Identity.Server.Interfaces
{
    public interface IUserSyncService
    {
        Task SyncExternalUserAsync(string externalUserId);
        Task SyncAllExternalUsersAsync();
    }
}