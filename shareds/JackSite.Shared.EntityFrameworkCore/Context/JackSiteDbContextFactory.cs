namespace JackSite.Shared.EntityFrameworkCore.Context;

/// <summary>
/// 数据库上下文工厂基类
/// </summary>
public abstract class JackSiteDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
    protected abstract string ConnectionStringName { get; }

    protected virtual void ConfigureDbContextOptions(DbContextOptionsBuilder<TContext> builder, string connectionString)
    {
        // 默认实现为空，由子类重写
    }

    public TContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString(ConnectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"未找到名为 '{ConnectionStringName}' 的连接字符串");
        }

        return CreateDbContext(connectionString);
    }

    private TContext CreateDbContext(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<TContext>();
        ConfigureDbContextOptions(builder, connectionString);

        return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options)!;
    }
}