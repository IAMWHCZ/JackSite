

namespace JackSite.Shared.EntityFrameworkCore.Extensions;

/// <summary>
/// DbContextOptionsBuilder 扩展方法
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// 根据提供程序类型配置数据库
    /// </summary>
    public static DbContextOptionsBuilder ConfigureByProvider(
        this DbContextOptionsBuilder optionsBuilder,
        DbProvider provider,
        string connectionString)
    {
        switch (provider)
        {
            case DbProvider.PostgreSQL:
                optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                break;
                
            case DbProvider.SqlServer:
                optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                break;
                
            case DbProvider.MySQL:
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                break;
                
            case DbProvider.SQLite:
                optionsBuilder.UseSqlite(connectionString, sqliteOptions =>
                {
                    // SQLite 不支持 EnableRetryOnFailure
                    sqliteOptions.MigrationsAssembly(System.Reflection.Assembly.GetCallingAssembly().GetName().Name);
                });
                break;
                
            default:
                throw new ArgumentOutOfRangeException(nameof(provider), provider, "不支持的数据库提供程序");
        }

        return optionsBuilder;
    }
}