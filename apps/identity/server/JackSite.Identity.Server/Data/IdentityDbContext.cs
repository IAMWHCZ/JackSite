namespace JackSite.Identity.Server.Data
{
    public partial class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
    {

        public DbSet<BaseProprietyEntity> Proprieties { get; set; }

        public DbSet<BaseKeyEntity> BaseKeyEntity { get; set; }

        public DbSet<SigningAlgorithm> SigningAlgorithm { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base
            .OnModelCreating(modelBuilder);

            modelBuilder.ApplyDataBaseNaming();
        }

    }
}
