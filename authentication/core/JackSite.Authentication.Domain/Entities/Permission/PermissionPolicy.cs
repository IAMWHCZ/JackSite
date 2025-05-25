using JackSite.Authentication.Entities.Actions;
using JackSite.Authentication.Entities.Resources;

namespace JackSite.Authentication.Entities.Permission;

public class PermissionPolicy : Entity
{
    public virtual Resource Resource { get; set; } = null!;

    public virtual ICollection<ActionBasic> ActionBasics { get; set; } = [];
    
    public virtual ICollection<PermissionPolicyCondition>? PermissionPolicyConditions { get; set; }
}