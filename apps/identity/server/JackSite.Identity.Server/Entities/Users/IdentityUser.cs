namespace JackSite.Identity.Server.Entities.Users
{
    public class IdentityUser
    {
        public string Id { get; set; } = string.Empty;

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
