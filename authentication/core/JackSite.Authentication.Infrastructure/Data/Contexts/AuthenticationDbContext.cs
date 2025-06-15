using JackSite.Authentication.Entities.Localization;
using JackSite.Authentication.Entities.Logs;
using JackSite.Authentication.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JackSite.Authentication.Infrastructure.Data;
public partial class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> dbContext):DbContext(dbContext)
{
    public DbSet<Translation>? Translations { get; set; }

    public DbSet<OperationLog>? OperationLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyNamingConventions();
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            var idProperty = entityType.FindProperty("Id");
            if (idProperty != null)
            {
                idProperty.ValueGenerated = ValueGenerated.Never;
            }
        }
        // 配置所有实体
        ConfigureUserEntities(modelBuilder);
        ConfigurePermissionEntities(modelBuilder);
        ConfigureEmails(modelBuilder);
        
        modelBuilder.Entity<OperationLog>(entity =>
        {
            entity.ToTable("operation_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ApiName)
                .HasMaxLength(256);
        
            entity.Property(e => e.Description)
                .HasMaxLength(512);
        
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50);
        
            entity.Property(e => e.UserAgent)
                .HasMaxLength(512);
        
            entity.Property(e => e.Browser)
                .HasMaxLength(50);
        
            entity.Property(e => e.Os)
                .HasMaxLength(50);
        
            entity.Property(e => e.Exception)
                .HasColumnType("text");
        });
    }
}
