using System.ComponentModel;

namespace JackSite.Identity.Server.Enums;

public static class RelationshipType
{
    public enum Scope : byte
    {
        [Description("Scope属性关联")]
        ScopeProperty = 1,

        [Description("Scope用户声明关联")]  
        ScopeUserDefine,
    }
    public enum RoleType : byte
    {
        [Description("用户角色关联")]
        UserRole
    }

    internal static class Source
    {
        public enum IdentitySource : byte
        {
            [Description("IdentitySourcePropriety关联")]
            IdentitySourcePropriety,
            [Description("IdentitySourceUserClaims关联")]
            IdentitySourceUserClaim
        }

        public enum ApiResource : byte
        {
            [Description("ApiResourceKeys关联")]
            ApiResourceKeys,
            [Description("ApiResourceProperties关联")]
            ApiResourceProperties,
            [Description("ApiResourceUserClaims关联")]
            ApiResourceUserClaims,
            [Description("ApiResourceScopes关联")]
            ApiResourceScopes,
            [Description("ApiResourceSigningAlgorithm关联")]
            ApiResourceSigningAlgorithm
        }

    }

    public enum Client : byte
    {
        [Description("ClientToken关联")]
        ClientToken,
        [Description("ClientPropriety关联")]
        ClientPropriety,
        [Description("Client跨域关联")]
        ClientCrossPolicy,
        [Description("ClientTokenDefine关联")]
        ClientTokenDefine,
        [Description("ClientScope关联")]
        ClientScope,
        [Description("ClientTokenSigningAlgorithm关联")]
        ClientTokenSigningAlgorithm,
        [Description("ClientKey关联")]
        ClientKey
    }

}
