using JackSite.Authentication.Entities.Clients;
using JackSite.Authentication.ValueObjects;

namespace JackSite.Authentication.Infrastructure.Data.Seeds;

public static class ClientSeeder
{
    public static ClientBasic Client => new()
    {
        
        Name = "JackSite",
        Description = "JackSite Authentication Client",
        Secret = "JackSite123456789"
    };
    
    public static ClientCorsOrigin CorsOrigin => new(Client.Id,"http://localhost:6066");
}