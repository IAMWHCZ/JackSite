using JackSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JackSite.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserBasic>
{
    public void Configure(EntityTypeBuilder<UserBasic> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.Salt)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasIndex(u => u.Username)
            .IsUnique();
        
        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}