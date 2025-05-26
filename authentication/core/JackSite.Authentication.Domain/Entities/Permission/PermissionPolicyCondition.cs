namespace JackSite.Authentication.Entities.Permission;

public class PermissionPolicyCondition:Entity
{
    public string Filter { get; set; } = null!;

    public string DataSource { get; set; } = null!;
}