namespace JackSite.Identity.Server.Data
{
    public partial class IdentityDbContext
    {
        public DbSet<Scope> Scopes { get; set; }
    }
}
