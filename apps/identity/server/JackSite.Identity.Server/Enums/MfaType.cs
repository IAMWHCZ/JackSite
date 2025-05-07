namespace JackSite.Identity.Server.Enums
{
    public enum MfaType
    {
        None = 1,
        TOTP,
        SMS,
        Email
    }
}