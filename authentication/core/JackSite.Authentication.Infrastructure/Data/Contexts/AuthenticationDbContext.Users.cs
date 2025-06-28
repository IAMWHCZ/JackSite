using JackSite.Authentication.Entities.UserGroups;

namespace JackSite.Authentication.Infrastructure.Data.Contexts;

public partial class AuthenticationDbContext
{
    public DbSet<UserBasic>? UserBasics { get; set; }
    public DbSet<UserProfile>? UserProfiles { get; set; }
    public DbSet<UserSettings>? UserSettings { get; set; }
    public DbSet<UserSecurityLog>? UserSecurityLogs { get; set; }
    public DbSet<UserGroup>? UserGroups { get; set; }
    public DbSet<UserGroupReference>? UserGroupReferences { get; set; }
    public DbSet<UserAttribute>? UserAttributes { get; set; }
    public DbSet<UserCredential>? UserCredentials { get; set; }
    public DbSet<UserSession>? UserSessions { get; set; }

    private static void ConfigureUserEntities(ModelBuilder modelBuilder)
    {
        // 配置 UserBasic 实体
        modelBuilder.Entity<UserBasic>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            entity.Property(e => e.Salt)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            entity.Property(e => e.LastLoginIp)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            // 软删除过滤器
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // 配置 UserProfile 实体
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.UserId).IsUnique();

            entity.Property(e => e.RealName)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.Street)
                .HasMaxLength(200);

            entity.Property(e => e.City)
                .HasMaxLength(100);

            entity.Property(e => e.Province)
                .HasMaxLength(100);

            entity.Property(e => e.Country)
                .HasMaxLength(100);

            entity.Property(e => e.PostalCode)
                .HasMaxLength(20);

            // 配置与 UserBasic 的关系
            entity.HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<UserProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 软删除过滤器
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // 配置 UserSettings 实体
        modelBuilder.Entity<UserSettings>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.UserId).IsUnique();

            entity.Property(e => e.TwoFactorType)
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.DateFormat)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            entity.Property(e => e.TimeFormat)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            // 配置与 UserBasic 的关系
            entity.HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<UserSettings>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置 UserSecurityLog 实体
        modelBuilder.Entity<UserSecurityLog>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.UserId);


            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            entity.Property(e => e.UserAgent)
                .HasMaxLength(500);

            // 配置与 UserBasic 的关系
            entity.HasOne<UserBasic>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserAttribute>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.UserId, e.Name }).IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            // 配置与 UserBasic 的关系
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserAttribute)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置 UserCredential 实体
        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.UserId).IsUnique();

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            entity.Property(e => e.Salt)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("UTC_TIMESTAMP()"); // MySQL 使用 UTC_TIMESTAMP()

            // 配置与 UserBasic 的关系
            entity.HasOne(e => e.User)
                .WithOne(u => u.UserCredential)
                .HasForeignKey<UserCredential>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

// 配置 UserSession 实体
        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.UserId, e.IsActive });

            entity.Property(e => e.IpAddress)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            entity.Property(e => e.StartTime)
                .IsRequired()
                .HasDefaultValueSql("UTC_TIMESTAMP()"); // MySQL 使用 UTC_TIMESTAMP()

            entity.Property(e => e.LastAccess)
                .IsRequired()
                .HasDefaultValueSql("UTC_TIMESTAMP()"); // MySQL 使用 UTC_TIMESTAMP()

            entity.Property(e => e.ExpiresAt)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // 配置与 UserBasic 的关系
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserSessions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}