namespace JackSite.Authentication.Entities.UI;
public class NavigationMenu:Entity,IOrderable
{
    /// <summary>
    /// 菜单项的显示名称（国际化键）
    /// </summary>
    public string NameKey { get; set; } = string.Empty;

    /// <summary>
    /// 菜单项的默认显示名称（当国际化资源不可用时）
    /// </summary>
    public string DefaultName { get; set; } = string.Empty;

    /// <summary>
    /// 父菜单ID
    /// </summary>
    public long? ParentId { get; set; } = 1;
    /// <summary>
    /// 菜单项的路由路径
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 子菜单项
    /// </summary>
    public virtual ICollection<NavigationMenu> Children { get; set; } = [];

    public int Order { get; set; }
}
