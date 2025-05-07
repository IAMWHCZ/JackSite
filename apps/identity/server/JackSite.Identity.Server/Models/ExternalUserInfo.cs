namespace JackSite.Identity.Server.Models
{
    public class ExternalUserInfo
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }  = string.Empty;
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = [];
        
        public string Provider { get; set; } = string.Empty;
    }
}