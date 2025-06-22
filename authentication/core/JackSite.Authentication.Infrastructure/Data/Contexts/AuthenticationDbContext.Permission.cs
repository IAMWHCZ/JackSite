using JackSite.Authentication.Entities.Permission;
using JackSite.Authentication.Entities.Resources;
using JackSite.Authentication.Entities.Roles;
using JackSite.Authentication.Entities.UserGroups;
using Role = JackSite.Authentication.Entities.Roles.Role;

namespace JackSite.Authentication.Infrastructure.Data.Contexts;

public partial class AuthenticationDbContext
{
    public DbSet<ActionBasic>? ActionBasics { get; set; }
    public DbSet<PermissionModel>? PermissionModels { get; set; } 
    public DbSet<PermissionPolicy>? PermissionPolicies { get; set; }
    public DbSet<PermissionPolicyCondition>? PermissionPolicyConditions { get; set; }
    public DbSet<Resource>? Resources { get; set; }
    public DbSet<Role>? Roles { get; set; }
    public DbSet<RoleReference>? RoleReferences { get; set; }

    private static void ConfigurePermissionEntities(ModelBuilder modelBuilder)
    {
        // 配置 ActionBasic 实体
        modelBuilder.Entity<ActionBasic>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ActionName)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.ActionDescription)
                .HasMaxLength(200);
        });
        
        // 配置 PermissionModel 实体
        modelBuilder.Entity<PermissionModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.FilName)
                .IsRequired()
                .HasMaxLength(100);
                
            // 软删除过滤器
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
        
        // 配置 Resource 实体
        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ResourceName)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Description)
                .HasMaxLength(200);
                
            entity.Property(e => e.Path)
                .IsRequired()
                .HasMaxLength(200);
        });
        
        // 配置 Role 实体
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.Description)
                .HasMaxLength(200);
        });
        
        // 配置 RoleReference 实体
        modelBuilder.Entity<RoleReference>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // 配置与 Role 的关系
            entity.HasOne<Role>()
                .WithMany(r => r.RoleReferences)  // 使用 Role 中的 RoleReferences 导航属性
                .HasForeignKey(r => r.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // 配置与 UserGroup 的关系
            entity.HasOne<UserGroup>()
                .WithMany()  // UserGroup 中没有对应的导航属性
                .HasForeignKey(r => r.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // 配置 PermissionPolicy 实体
        modelBuilder.Entity<PermissionPolicy>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // 配置与 Resource 的关系
            entity.HasOne(p => p.Resource)
                .WithMany()
                .HasForeignKey("ResourceId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            // 配置与 ActionBasic 的多对多关系
            entity.HasMany(p => p.ActionBasics)
                .WithMany()
                .UsingEntity(j => j.ToTable("permission_policy_actions"));
            
            // 配置与 PermissionPolicyCondition 的一对多关系
            entity.HasMany(p => p.PermissionPolicyConditions)
                .WithOne()
                .HasForeignKey("PolicyId")
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // 配置 PermissionPolicyCondition 实体
        modelBuilder.Entity<PermissionPolicyCondition>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // 添加外键关系
            entity.HasOne<PermissionPolicy>()
                .WithMany()
                .HasForeignKey("PolicyId")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

}