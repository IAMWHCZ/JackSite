using JackSite.Identity.Server.Configuration;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Identity.Server.Data
{
    public partial class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base
            .OnModelCreating(modelBuilder);

            modelBuilder.ApplyDataBaseNaming();
        }

    }
}
