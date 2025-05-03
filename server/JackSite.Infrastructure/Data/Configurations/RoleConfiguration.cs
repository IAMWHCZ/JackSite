using JackSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JackSite.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(r => r.Description)
            .HasMaxLength(200);
        
        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasIndex(r => r.Name)
            .IsUnique();
        
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}