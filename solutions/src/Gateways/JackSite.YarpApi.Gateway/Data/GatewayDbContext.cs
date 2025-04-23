using JackSite.Infrastructure.Data;
using JackSite.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using JackSite.YarpApi.Gateway.Entities;

namespace JackSite.YarpApi.Gateway.Data;

public class GatewayDbContext(DbContextOptions<GatewayDbContext> options) : JackSiteDbContext<GatewayDbContext>(options)
{
    internal DbSet<RequestLog> RequestLogs { get; set; } = null!;
    internal DbSet<YarpConfig> YarpConfigs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GatewayDbContext).Assembly);
        modelBuilder.Entity<YarpConfig>(entity =>
       {
           entity.HasKey(e => e.Id);
           entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
           entity.Property(e => e.Description).HasMaxLength(500);
           entity.Property(e => e.ConfigJson).IsRequired();
           // 确保只有一个活动配置
           entity.HasIndex(e => e.IsActive);
       });

        // CamelCase to SnakeCase
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Replace table names
            if (!string.IsNullOrWhiteSpace(entity.GetTableName()))
            {
                entity.SetTableName(entity.GetTableName()!.ToSnakeCase());
            }

            // Replace column names            
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                if (!string.IsNullOrWhiteSpace(key.GetName()))
                {
                    key.SetName(key.GetName()!.ToSnakeCase());
                }
            }

            foreach (var key in entity.GetForeignKeys())
            {
                if (!string.IsNullOrWhiteSpace(key.GetConstraintName()))
                {
                    key.SetConstraintName(key.GetConstraintName()!.ToSnakeCase());
                }
            }

            foreach (var index in entity.GetIndexes())
            {
                if (!string.IsNullOrWhiteSpace(index.GetDatabaseName()))
                {
                    index.SetDatabaseName(index.GetDatabaseName()!.ToSnakeCase());
                }
            }
        }
    }
}