namespace JackSite.Shared.EntityFrameworkCore.Configs;

/// <summary>
/// 实体配置基类
/// </summary>
public abstract class EntityTypeConfigurationBase<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
    }
}