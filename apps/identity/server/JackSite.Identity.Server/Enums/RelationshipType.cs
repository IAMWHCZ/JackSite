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
        ClientTokenSigningAlgorithm
    }

}
