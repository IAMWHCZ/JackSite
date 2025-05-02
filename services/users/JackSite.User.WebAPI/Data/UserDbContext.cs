using JackSite.Shared.EntityFrameworkCore.Converters;

namespace JackSite.User.WebAPI.Data;

/// <summary>
/// 用户数据库上下文
/// </summary>
public partial class UserDbContext : DbContext
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">数据库上下文选项</param>
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // 配置 SnowflakeId 类型使用 long 存储
        configurationBuilder
            .Properties<SnowflakeId>()
            .HaveConversion<SnowflakeIdValueConverter>();

        base.ConfigureConventions(configurationBuilder);
    }

    /// <summary>
    /// 配置模型
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ApplyConfigurationsFromAssembly(GetType().Assembly)
            .ConfigureSnowflakeId()
            .ApplySoftDeleteFilter()
            .ConfigureSnakeCase();

        ConfigureUserEntities(modelBuilder);
    }
}