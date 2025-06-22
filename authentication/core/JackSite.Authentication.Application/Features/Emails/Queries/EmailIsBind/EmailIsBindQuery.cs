namespace JackSite.Authentication.Application.Features.Emails;

/// <summary>
/// UserIsExist 命令
/// </summary>
public sealed record EmailIsBindQuery(string Email) : IQuery<bool>;