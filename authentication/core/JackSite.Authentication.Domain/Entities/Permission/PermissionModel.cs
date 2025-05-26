namespace JackSite.Authentication.Entities.Permission;

public class PermissionModel:Entity,ISoftDeletable
{
    public string FilName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}