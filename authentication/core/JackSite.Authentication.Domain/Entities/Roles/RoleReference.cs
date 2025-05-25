namespace JackSite.Authentication.Entities.Roles;

public class RoleReference:Entity
{
    public long RoleId { get; set; }
    public long GroupId  { get; set; }
}