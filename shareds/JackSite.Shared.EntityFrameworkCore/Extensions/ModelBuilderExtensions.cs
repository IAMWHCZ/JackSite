using System.Reflection;
using JackSite.Shared.Core.IdGenerator;
using JackSite.Shared.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JackSite.Shared.EntityFrameworkCore.Extensions;

/// <summary>
/// ModelBuilder 扩展方法
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// 应用软删除过滤器
    /// </summary>
    public static ModelBuilder ApplySoftDeleteFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // 检查实体是否继承自 SoftDeleteEntityBase
            if (typeof(Entities.SoftDeleteEntityBase<>).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, "IsDeleted");
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        return modelBuilder;
    }

    /// <summary>
    /// 应用所有配置
    /// </summary>
    public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder, Assembly assembly)
    {
        var configTypes = assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsGenericType: false } &&
                           type.GetInterfaces().Any(i => i.IsGenericType &&
                                                         i.GetGenericTypeDefinition() ==
                                                         typeof(IEntityTypeConfiguration<>)));

        foreach (var configType in configTypes)
        {
            var config = Activator.CreateInstance(configType);
            if (config != null) modelBuilder.ApplyConfiguration((dynamic)config);
        }

        return modelBuilder;
    }

    /// <summary>
    /// 配置 SnowflakeId 类型转换
    /// </summary>
    public static ModelBuilder ConfigureSnowflakeId(this ModelBuilder modelBuilder)
    {
        // 创建 SnowflakeId 值转换器
        var converter = new ValueConverter<SnowflakeId, long>(
            v => v.ToInt64(),
            v => new SnowflakeId(v)
        );
        
        // 为所有 SnowflakeId 类型的属性应用转换器
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(SnowflakeId))
                {
                    property.SetValueConverter(converter);
                }
            }
        }

        return modelBuilder;
    }

    /// <summary>
    /// 配置驼峰命名转换
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    public static ModelBuilder ConfigureSnakeCase(this ModelBuilder modelBuilder)
    {
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

        return modelBuilder;
    }
}