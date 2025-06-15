namespace JackSite.Authentication.Base;

public record BaseHeaderParams(
    LanguageType Language,
    string UserName,
    string Email,
    long UserId
);