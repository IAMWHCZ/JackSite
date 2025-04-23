using System.Text.Json;
using JackSite.Common.Domain;
using JackSite.YarpApi.Gateway.Const;
using JackSite.YarpApi.Gateway.Data;
using JackSite.YarpApi.Gateway.Entities;
using Microsoft.EntityFrameworkCore;
using Yarp.ReverseProxy.Configuration;

namespace JackSite.YarpApi.Gateway.Extensions;

public static class DatabaseExtensions
{
    public static async Task EnsureDatabaseAsync(this IApplicationBuilder app, ILogger logger,IConfiguration configuration)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GatewayDbContext>();
            var proxyConfigProvider = scope.ServiceProvider.GetRequiredService<IProxyConfigProvider>();
            logger.LogInformation("Ensuring database exists and is up to date");
            await dbContext.Database.MigrateAsync();
            
            // 检查是否需要添加种子配置
            if (!await dbContext.YarpConfigs.AnyAsync())
            {
                logger.LogInformation("Adding seed YARP configuration");
                await AddSeedYarpConfigAsync(dbContext, proxyConfigProvider);
            }
            
            logger.LogInformation("Database check completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while ensuring the database exists");
            throw;
        }
    }

    private static async Task AddSeedYarpConfigAsync(GatewayDbContext dbContext,IProxyConfigProvider proxyConfigProvider)
    {
        var cluster = proxyConfigProvider.GetConfig().Clusters;
        var route = proxyConfigProvider.GetConfig().Routes;
        
        var exist =  await dbContext.YarpConfigs
        .AsNoTracking()
        .AnyAsync(x=>x.Id == DatabaseConst.SnowflakeId);
        if (exist) return;
        var yarpConfig = new YarpConfig
        {
            Id = DatabaseConst.SnowflakeId,
            Name = "Initial Configuration",
            Description = "Initial YARP configuration from appsettings.json",
            IsActive = true,
            LastModified = DateTime.UtcNow,
            ConfigJson = JsonSerializer.Serialize(new
            {
                Routes = route,
                Clusters = cluster,
            })
        };

        dbContext.YarpConfigs.Add(yarpConfig);
        await dbContext.SaveChangesAsync();
    }
}
