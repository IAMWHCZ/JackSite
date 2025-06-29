namespace JackSite.Authentication.Enums.Clients;

public enum ClientProtocolType : byte
{
    
    [Description("OpenID Connect 协议")]
    OpenIdConnect = 1,
    
    [Description("OAuth 2.0 协议")]
    OAuth2,
    
    [Description("SAML 2.0 协议")]
    Saml2,
    
    [Description("WS-Federation 协议")]
    WsFed
}