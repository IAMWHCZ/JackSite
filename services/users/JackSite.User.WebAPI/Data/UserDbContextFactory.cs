using JackSite.Shared.EntityFrameworkCore.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JackSite.User.WebAPI.Data;

/// <summary>
/// 用户数据库上下文工厂
/// </summary>
public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    /// <summary>
    /// 创建数据库上下文
    /// </summary>
    public UserDbContext CreateDbContext(string[] args)
    {
        // 加载配置
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // 获取连接字符串
        var connectionString = configuration.GetConnectionString("SqlServer");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("未找到 SqlServer 连接字符串");
        }

        // 创建选项构建器
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        
        // 配置数据库
        ConfigureDbContextOptions(optionsBuilder, connectionString);

        // 创建上下文
        return new UserDbContext(optionsBuilder.Options);
    }

    /// <summary>
    /// 配置数据库上下文选项
    /// </summary>
    protected virtual void ConfigureDbContextOptions(DbContextOptionsBuilder<UserDbContext> builder, string connectionString)
    {
        // 使用 SQL Server 数据库
        builder.UseSqlServer(connectionString, options =>
        {
            // 配置重试策略
            options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            
            // 配置命令超时
            options.CommandTimeout(30);
            
            // 使用当前程序集的迁移
            options.MigrationsAssembly(typeof(UserDbContext).Assembly.GetName().Name);
        });
        
        // 启用敏感数据日志（仅在开发环境）
        builder.EnableSensitiveDataLogging();
        
        // 启用详细错误消息
        builder.EnableDetailedErrors();
    }
}