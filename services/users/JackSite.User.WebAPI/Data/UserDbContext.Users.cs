namespace JackSite.User.WebAPI.Data;

/// <summary>
/// 用户数据库上下文 - 用户相关实体
/// </summary>
public partial class UserDbContext
{
    /// <summary>
    /// 用户表
    /// </summary>
    public DbSet<Entities.Users.User> Users { get; set; } = null!;
    
    /// <summary>
    /// 用户组表
    /// </summary>
   
    /// <summary>
    /// 配置用户相关实体
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    private void ConfigureUserEntities(ModelBuilder modelBuilder)
    {
       
        
    }
}