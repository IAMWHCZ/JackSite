using JackSite.Authentication.Entities.Localization;
using JackSite.Authentication.Infrastructure.Extensions;

namespace JackSite.Authentication.Infrastructure.Data;

public partial class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> dbContext):DbContext(dbContext)
{
    public DbSet<Translation>? Translations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyNamingConventions();
        // 配置所有实体
        ConfigureUiEntities(modelBuilder);
        ConfigureUserEntities(modelBuilder);
        ConfigurePermissionEntities(modelBuilder);
    }
}
