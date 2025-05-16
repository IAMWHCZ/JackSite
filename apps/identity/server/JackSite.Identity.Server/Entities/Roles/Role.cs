namespace JackSite.Identity.Server.Entities.Roles
{
    public class Role:BaseEntity
    {
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
    }
}