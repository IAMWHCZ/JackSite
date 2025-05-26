using JackSite.Authentication.Entities.UI;

namespace JackSite.Authentication.Infrastructure.Data;

public partial class AuthenticationDbContext
{
    public DbSet<NavigationMenu>? NavigationMenus { get; set; } = null!;
    
    private static void ConfigureUiEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NavigationMenu>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.NameKey)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.DefaultName)
                .HasMaxLength(100);
                
            entity.Property(e => e.Path)
                .HasMaxLength(20);
                
            // 配置自引用关系
            entity.HasOne<NavigationMenu>()
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });
    }
}