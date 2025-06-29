namespace JackSite.Authentication.Entities.Permission;

public class PermissionModel : Entity, ISoftDeletable
{
    [Description("文件名")]
    public string FilName { get; set; } = string.Empty;

    [Description("是否已删除")]
    public bool IsDeleted { get; set; }

    [Description("删除时间")]
    public DateTimeOffset? DeletedOnUtc { get; set; }
}