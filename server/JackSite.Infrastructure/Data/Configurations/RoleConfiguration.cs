namespace JackSite.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        
        builder.HasKey(r => r.Id);
        
        
        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasIndex(r => r.Name)
            .IsUnique();
        
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}