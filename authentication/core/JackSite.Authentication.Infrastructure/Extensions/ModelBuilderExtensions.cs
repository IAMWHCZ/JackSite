using System.ComponentModel;
using System.Reflection;

namespace JackSite.Authentication.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyNamingConventions(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
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
        return builder;
    }
    
    public static void ApplyDescriptionAsComments(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var clrProperty = property.PropertyInfo;
                if (clrProperty != null)
                {
                    var descriptionAttribute = clrProperty.GetCustomAttribute<DescriptionAttribute>();
                    if (descriptionAttribute != null)
                    {
                        property.SetComment(descriptionAttribute.Description);
                    }
                }
            }
        }
    }
}