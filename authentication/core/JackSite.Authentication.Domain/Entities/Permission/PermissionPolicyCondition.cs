namespace JackSite.Authentication.Entities.Permission;

public class PermissionPolicyCondition : Entity
{
    [Description("过滤条件")]
    public string Filter { get; set; } = null!;

    [Description("数据源")]
    public string DataSource { get; set; } = null!;
}