namespace JackSite.Identity.Server.Enums
{
    public enum TokenSigningAlgorithmType:byte
    {
        None = 1,
        ES256, 
        ES384,
        ES512,
        PS256,
        PS384,
        PS512,
        RS256,
        RS384,
        RS512
    }
}
