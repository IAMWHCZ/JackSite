using System.Collections.Generic;

namespace JackSite.Authentication.PermissionServer.Models
{
    public class GetPermissionsQuery
    {
        public long UserId { get; set; }

        public IReadOnlyList<string> Resources { get; set; } = new List<string>();
    }
}