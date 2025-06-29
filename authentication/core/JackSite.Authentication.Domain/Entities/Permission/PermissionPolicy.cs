namespace JackSite.Authentication.Entities.Permission;

public class PermissionPolicy : Entity
{
    [Description("关联资源")]
    public virtual Resource Resource { get; set; } = null!;

    [Description("操作基础信息集合")]
    public virtual ICollection<ActionBasic> ActionBasics { get; set; } = [];

    [Description("权限策略条件集合")]
    public virtual ICollection<PermissionPolicyCondition>? PermissionPolicyConditions { get; set; }

    [Description("权限模型")]
    public virtual PermissionModel Model { get; set; } = null!;
}