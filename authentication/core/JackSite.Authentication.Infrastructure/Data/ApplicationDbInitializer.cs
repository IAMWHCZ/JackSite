using JackSite.Authentication.Abstractions;
using JackSite.Authentication.Infrastructure.Data.Contexts;
using JackSite.Authentication.Infrastructure.Data.Seeds;

namespace JackSite.Authentication.Infrastructure.Data;

public class ApplicationDbInitializer(AuthenticationDbContext dnContext):IApplicationDbInitializer
{
    
    public async Task SeedAsync()
    {
        // 确保数据库已创建
        await dnContext.Database.EnsureCreatedAsync();

        // 检查是否已经有数据
        if (!dnContext.ClientBasics.Any())
        {
            // 先添加 Client 并保存以获取 ID
            var client = ClientSeeder.Client;
            dnContext.ClientBasics.Add(client);
            await dnContext.SaveChangesAsync();

            // 使用保存后的 client.Id 设置 CorsOrigin 的 client_id
            var corsOrigin = ClientSeeder.CorsOrigin;
            corsOrigin.ClientId = client.Id; // 确保使用正确的 ID
        
            dnContext.ClientCorsOrigins.Add(corsOrigin);
            await dnContext.SaveChangesAsync();
        }
    }
}