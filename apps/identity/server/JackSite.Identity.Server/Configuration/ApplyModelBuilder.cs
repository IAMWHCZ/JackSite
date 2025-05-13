using JackSite.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace JackSite.Identity.Server.Configuration
{
    public static class ApplyModelBuilder
    {
        public static ModelBuilder ApplyDataBaseNaming(this ModelBuilder builder) {
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
    }
}
