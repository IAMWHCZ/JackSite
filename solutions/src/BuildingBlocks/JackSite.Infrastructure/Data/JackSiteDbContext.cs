using JackSite.Infrastructure.Extensions;

namespace JackSite.Infrastructure.Data;
public class JackSiteDbContext<TContext>(DbContextOptions<TContext> options) : DbContext(options)
    where TContext : DbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<SnowflakeId>()
            .HaveConversion<SnowflakeIdConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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
        // Configure all properties of type SnowflakeId to use the converter
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(SnowflakeId));

            foreach (var property in properties)
            {
                property.SetValueConverter(new SnowflakeIdConverter());
            }
        }
    }
}