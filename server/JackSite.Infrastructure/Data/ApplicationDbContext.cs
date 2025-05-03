namespace JackSite.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
    }
}