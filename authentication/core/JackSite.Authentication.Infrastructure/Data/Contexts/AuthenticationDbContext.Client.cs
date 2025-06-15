using JackSite.Authentication.Entities.Clients;

namespace JackSite.Authentication.Infrastructure.Data.Contexts;

public partial class AuthenticationDbContext
{
    public DbSet<ClientBasic> ClientBasics { get; set; }
    public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
    public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
    public DbSet<ClientScope> ClientScopes { get; set; }
    public DbSet<ClientSession> ClientSessions { get; set; }
    
    private static void ConfigureClientEntities(ModelBuilder modelBuilder)
    {
        // 配置 ClientBasic 实体
        modelBuilder.Entity<ClientBasic>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Description)
                .HasMaxLength(500);
                
            entity.Property(e => e.Secret)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Protocol)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("openid-connect");
                
            // 配置与关联实体的关系
            entity.HasMany(e => e.RedirectUris)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasMany(e => e.AllowedScopes)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasMany(e => e.AllowedCorsOrigins)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // 配置 ClientRedirectUri 实体
        modelBuilder.Entity<ClientRedirectUri>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Uri)
                .IsRequired()
                .HasMaxLength(2000);
        });
        
        // 配置 ClientScope 实体
        modelBuilder.Entity<ClientScope>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Scope)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        // 配置 ClientCorsOrigin 实体
        modelBuilder.Entity<ClientCorsOrigin>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Origin)
                .IsRequired()
                .HasMaxLength(2000);
        });
        
        // 配置 ClientSession 实体
        modelBuilder.Entity<ClientSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // 配置关系
            entity.HasOne(e => e.UserSession)
                .WithMany()
                .HasForeignKey(e => e.UserSessionId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}