namespace JackSite.Authentication.Entities.Permission;

public class PermissionPolicyCondition:Entity
{
    public string Filter { get; set; }

    public string DataSource { get; set; }
}