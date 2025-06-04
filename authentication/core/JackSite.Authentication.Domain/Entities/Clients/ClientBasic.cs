namespace JackSite.Authentication.Entities.Clients;

public class ClientBasic : Entity
{
    public string ClientId { get; private set; } = null!; // 客户端标识符
    public string Name { get; private set; } = null!; // 显示名称
    public string? Description { get; private set; }
    public string Secret { get; private set; } = null!; // 客户端密钥
    public bool Enabled { get; private set; } = true;
    public bool Confidential { get; private set; } // 是否为机密客户端
    public string Protocol { get; private set; } = "openid-connect"; // 协议类型
    public int AccessTokenLifespan { get; private set; } = 300; // 访问令牌有效期（秒）
    public int RefreshTokenLifespan { get; private set; } = 1800; // 刷新令牌有效期（秒）
    
    // 关联
    public virtual ICollection<ClientRedirectUri> RedirectUris { get; private set; } = [];
    public virtual ICollection<ClientScope> AllowedScopes { get; private set; } = [];
    public virtual ICollection<ClientCorsOrigin> AllowedCorsOrigins { get; private set; } = [];
}