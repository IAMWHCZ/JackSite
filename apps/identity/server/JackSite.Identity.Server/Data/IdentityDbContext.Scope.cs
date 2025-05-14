namespace JackSite.Identity.Server.Data
{
    public partial class IdentityDbContext
    {
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<ScopeProperty> ScopeProperties { get; set; }
        public DbSet<ScopeUserDefine> ScopeUserDefines { get; set; }
        public DbSet<ScopeRecommendationDefine> ScopeRecommendationDefines { get; set; }
    }
}
