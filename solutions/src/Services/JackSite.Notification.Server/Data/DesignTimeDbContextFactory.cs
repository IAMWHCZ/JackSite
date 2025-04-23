using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JackSite.Notification.Server.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    public NotificationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
        var connectionString = configuration.GetSection("MSSQL:ConnectionString").Value;
        
        optionsBuilder.UseSqlServer(connectionString);

        return new NotificationDbContext(optionsBuilder.Options);
    }
}