using Microsoft.EntityFrameworkCore.Design;

namespace JackSite.Authentication.Infrastructure.Data.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Contexts.AuthenticationDbContext>
{
    public Contexts.AuthenticationDbContext CreateDbContext(string[] args)
    {
            
        var builder = new DbContextOptionsBuilder<Contexts.AuthenticationDbContext>();
        builder.UseMySQL("Server=localhost;Port=3306;Database=JackSiteAuthorizationDB;Uid=root;Pwd=Cz18972621866;");
        return new Contexts.AuthenticationDbContext(builder.Options);
    }
}