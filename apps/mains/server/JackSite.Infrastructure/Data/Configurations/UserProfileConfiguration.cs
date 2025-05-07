using JackSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JackSite.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        // 配置地址字段
        builder.Property(p => p.Street).HasMaxLength(200).IsRequired(false);
        builder.Property(p => p.City).HasMaxLength(100).IsRequired(false);
        builder.Property(p => p.Province).HasMaxLength(100).IsRequired(false);
        builder.Property(p => p.Country).HasMaxLength(100).IsRequired(false);
        builder.Property(p => p.PostalCode).HasMaxLength(20).IsRequired(false);
        
        // 配置软删除过滤器
        builder.HasQueryFilter(p => !p.IsDeleted);
        
        // 配置与 UserBasic 的关系
        builder.HasOne(p => p.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}