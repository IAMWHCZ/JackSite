namespace JackSite.UserServer.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : JackSiteDbContext<UserDbContext>(options)
{
    // 添加你的 DbSet 属性
    internal DbSet<User> Users { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);    
        // 添加实体配置
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}