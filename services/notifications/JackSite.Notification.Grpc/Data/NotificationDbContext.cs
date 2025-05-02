using JackSite.Notification.Grpc.Entities;
using JackSite.Shared.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Notification.Grpc.Data;

/// <summary>
/// 通知服务数据库上下文
/// </summary>
public class NotificationDbContext : DbContext
{
    /// <summary>
    /// 邮件日志
    /// </summary>
    public DbSet<EmailLog> EmailLogs { get; set; } = null!;
    
    /// <summary>
    /// 构造函数
    /// </summary>
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }
    
    /// <summary>
    /// 配置模型
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.ConfigureSnakeCase();
        modelBuilder.ConfigureSnowflakeId();
    }
}