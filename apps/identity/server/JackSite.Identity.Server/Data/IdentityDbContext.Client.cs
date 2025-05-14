namespace JackSite.Identity.Server.Data
{
    public partial class IdentityDbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientBasic> ClientBasics { get; set; }
        public DbSet<ClientAllowScreen> ClientAllowScreens { get; set; }
        public DbSet<ClientAuthorizationCategory> ClientAuthorizationCategories { get; set; }
        public DbSet<ClientCertificationCancel> ClientCertificationCancels { get; set; }
        public DbSet<ClientCrossPolicy> ClientCrossPolicies { get; set; }
        public DbSet<ClientDeviceWorkflow> ClientDeviceWorkflows { get; set; }
        public DbSet<ClientKey> ClientKeys { get; set; }
        public DbSet<ClientPropriety> ClientProprieties { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public DbSet<ClientToken> ClientToken { get; set; }
        public DbSet<ClientTokenDefine> ClientTokenDefines { get; set; }
        public DbSet<ClientTokenSigningAlgorithm> ClientTokenSigningAlgorithms { get; set; }
    }
}
