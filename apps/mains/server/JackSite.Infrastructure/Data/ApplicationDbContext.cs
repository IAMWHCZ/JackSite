

namespace JackSite.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    internal DbSet<UserBasic> Users { get; set; } = null!;
    internal DbSet<LogEntry> Logs { get; set; } = null!;
    internal DbSet<UserProfile> UserProfiles { get; set; } = null!;
    internal DbSet<UserSettings> UserSettings { get; set; } = null!;
    internal DbSet<UserSecurityLog> UserSecurityLogs { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ApplyConfigurationsFromAssembly(GetType().Assembly)
            .ApplyNamingConventions();
    }
}