namespace JackSite.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserBasic>
{
    public void Configure(EntityTypeBuilder<UserBasic> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        
        // 软删除过滤器
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}