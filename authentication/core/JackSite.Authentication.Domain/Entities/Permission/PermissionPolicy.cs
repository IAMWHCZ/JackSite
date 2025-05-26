namespace JackSite.Authentication.Entities.Permission;

public class PermissionPolicy : Entity
{
    public virtual Resource Resource { get; set; } = null!;

    public virtual ICollection<ActionBasic> ActionBasics { get; set; } = [];
    
    public virtual ICollection<PermissionPolicyCondition>? PermissionPolicyConditions { get; set; }

    public virtual PermissionModel Model { get; set; } = null!;
}