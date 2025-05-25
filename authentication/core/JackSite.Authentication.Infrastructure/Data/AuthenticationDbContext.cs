namespace JackSite.Authentication.Infrastructure.Data;

public partial class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> dbContext):DbContext(dbContext)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 配置所有实体
        ConfigureUiEntities(modelBuilder);
        ConfigureUserEntities(modelBuilder);
    }
}
