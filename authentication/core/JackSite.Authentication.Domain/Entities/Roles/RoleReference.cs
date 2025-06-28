namespace JackSite.Authentication.Entities.Roles;

public class RoleReference:Entity
{
    public Guid RoleId { get; set; }
    public Guid GroupId  { get; set; }
}