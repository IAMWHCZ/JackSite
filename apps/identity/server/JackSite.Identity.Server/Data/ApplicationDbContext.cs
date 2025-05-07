using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JackSite.Identity.Server.Models;

namespace JackSite.Identity.Server.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Configure RefreshToken entity
            builder.Entity<RefreshToken>()
                .HasKey(r => r.Id);
                
            builder.Entity<RefreshToken>()
                .HasIndex(r => r.Token)
                .IsUnique();
        }
    }
}