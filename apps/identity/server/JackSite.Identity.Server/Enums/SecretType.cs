namespace JackSite.Identity.Server.Enums
{
    public enum SecretType:byte
    {
        SharedSecret = 1,
        X509Certificate,
        X509Name,
        X509Thumbprint
    }
}