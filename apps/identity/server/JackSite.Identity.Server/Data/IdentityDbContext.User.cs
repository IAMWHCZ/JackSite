using JackSite.Identity.Server.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Data
{
    public partial class IdentityDbContext
    {
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityUserInformation> UserInformations { get; set; }
        public DbSet<IdentityUserProfile> UserProfiles { get; set; }
        public DbSet<IdentityUserSecurity> UserSecurities { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
    }
}
