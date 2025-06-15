namespace JackSite.Authentication.Entities.Clients;

public class ClientBasic : Entity
{
    public string Name { get; set; } = null!; // 显示名称
    public string? Description { get; set; }
    public string Secret { get; set; } = null!; // 客户端密钥
    public bool Enabled { get; set; } = true;
    public bool Confidential { get; set; } = true; // 是否为机密客户端
    public string Protocol { get; set; } = "openid-connect"; // 协议类型
    public int AccessTokenLifespan { get; set; } = 3600; // 访问令牌有效期（秒）- 1小时
    public int RefreshTokenLifespan { get; set; } = 604800; // 刷新令牌有效期（秒）- 7天
    
    // 关联
    public virtual ICollection<ClientRedirectUri> RedirectUris { get; set; } = [];
    public virtual ICollection<ClientScope> AllowedScopes { get; set; } = [];
    public virtual ICollection<ClientCorsOrigin> AllowedCorsOrigins { get; set; } = [];
}