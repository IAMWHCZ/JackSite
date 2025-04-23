using JackSite.Infrastructure.Data;
using JackSite.Infrastructure.Extensions;
using JackSite.Notification.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace JackSite.Notification.Server.Data;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) :JackSiteDbContext<NotificationDbContext>(options)
{
    internal DbSet<Entities.Notification> Notification { get; set; } 
    
    internal DbSet<ScheduledMessage>  ScheduledMessages { get; set; } 
    internal DbSet<Email> Emails { get; set; }
    
    internal DbSet<EmailContent> EmailContents { get; set; }
    
    internal DbSet<EmailAttachment> EmailAttachments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
        
        // CamelCase to SnakeCase
        foreach (var entity in modelBuilder.Model.GetEntityTypes()) {
            // Replace table names
            if (!string.IsNullOrWhiteSpace(entity.GetTableName())) {
                entity.SetTableName(entity.GetTableName()!.ToSnakeCase());
            }

            // Replace column names            
            foreach (var property in entity.GetProperties()) {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            foreach (var key in entity.GetKeys()) {
                if (!string.IsNullOrWhiteSpace(key.GetName())) {
                    key.SetName(key.GetName()!.ToSnakeCase());
                }
            }

            foreach (var key in entity.GetForeignKeys()) {
                if (!string.IsNullOrWhiteSpace(key.GetConstraintName())) {
                    key.SetConstraintName(key.GetConstraintName()!.ToSnakeCase());
                }
            }

            foreach (var index in entity.GetIndexes()) {
                if (!string.IsNullOrWhiteSpace(index.GetDatabaseName())) {
                    index.SetDatabaseName(index.GetDatabaseName()!.ToSnakeCase());
                }
            }
        }
    }
}