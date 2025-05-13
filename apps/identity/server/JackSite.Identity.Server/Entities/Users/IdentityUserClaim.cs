using JackSite.Identity.Server.Entities.Bases;

namespace JackSite.Identity.Server.Entities.Users
{
    public class UserClaim:BaseEntity
    {
        public string ClaimName { get; set; } = default!;
    }
}
