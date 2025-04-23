namespace JackSite.UserServer.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // 主键
        builder.HasKey(u => u.Id);

        // 用户名
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(u => u.Username)
            .IsUnique();

        // 邮箱
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(u => u.Email)
            .IsUnique();

        // 密码哈希
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);

        // 手机号
        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);
        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasFilter("[PhoneNumber] IS NOT NULL");

        // 昵称
        builder.Property(u => u.Nickname)
            .HasMaxLength(50);

        // 头像
        builder.Property(u => u.Avatar)
            .HasMaxLength(256);

        // 个人简介
        builder.Property(u => u.Bio)
            .HasMaxLength(500);

        // 状态
        builder.Property(u => u.Status)
            .IsRequired()
            .HasDefaultValue(UserStatus.Inactive);

        // IP地址
        builder.Property(u => u.LastLoginIp)
            .HasMaxLength(45);

        // 创建和更新时间
        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.UpdatedAt)
            .IsRequired(false)
            .ValueGeneratedOnUpdate();
    }
}