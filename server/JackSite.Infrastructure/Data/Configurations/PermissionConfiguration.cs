using JackSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JackSite.Infrastructure.Data.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.Description)
            .HasMaxLength(200);
        
        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasIndex(p => p.Code)
            .IsUnique();
        
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}