namespace JackSite.Identity.Server.Models
{
    public class SocialLoginOptions
    {
        public GoogleOptions Google { get; set; }
        public MicrosoftOptions Microsoft { get; set; }
        public FacebookOptions Facebook { get; set; }
        public GitHubOptions GitHub { get; set; }
    }
    
    public class GoogleOptions
    {
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
    
    public class MicrosoftOptions
    {
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
    }
    
    public class FacebookOptions
    {
        public bool Enabled { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
    
    public class GitHubOptions
    {
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}