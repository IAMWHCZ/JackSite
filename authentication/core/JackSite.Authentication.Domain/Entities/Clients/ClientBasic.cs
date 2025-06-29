using JackSite.Authentication.Enums.Clients;

namespace JackSite.Authentication.Entities.Clients;

public class ClientBasic : Entity
{
    [Description("客户端名称")]
    public string Name { get; set; } = null!; // 显示名称

    [Description("客户端描述")]
    public string? Description { get; set; }

    [Description("客户端密钥")]
    public string Secret { get; set; } = null!; // 客户端密钥

    [Description("是否启用")]
    public bool Enabled { get; set; } = true;

    [Description("是否为机密客户端")]
    public bool Confidential { get; set; } = true; // 是否为机密客户端

    [Description("协议类型")]
    public ClientProtocolType Protocol { get; set; } = ClientProtocolType.OpenIdConnect; // 协议类型

    [Description("访问令牌有效期（秒）")]
    public int AccessTokenLifespan { get; set; } = 3600; // 访问令牌有效期（秒）- 1小时

    [Description("刷新令牌有效期（秒）")]
    public int RefreshTokenLifespan { get; set; } = 604800; // 刷新令牌有效期（秒）- 7天
    
    // 关联
    [Description("重定向URI集合")]
    public virtual ICollection<ClientRedirectUri> RedirectUris { get; set; } = [];

    [Description("允许的作用域集合")]
    public virtual ICollection<ClientScope> AllowedScopes { get; set; } = [];

    [Description("允许的CORS源集合")]
    public virtual ICollection<ClientCorsOrigin> AllowedCorsOrigins { get; set; } = [];

    public virtual ICollection<ClientSession> ClientSessions { get; set; } = [];
}