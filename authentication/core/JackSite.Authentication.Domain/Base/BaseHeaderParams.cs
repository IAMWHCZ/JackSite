namespace JackSite.Authentication.Base;

public class BaseHeaderParams
{
    public LanguageType Language { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public long UserId { get; set; }
}