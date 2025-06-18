namespace JackSite.Authentication.Application.Features.Emails.Queries.EmailIsBind;

/// <summary>
/// UserIsExist 命令
/// </summary>
public sealed record EmailIsBindQuery(string Email) : IQuery<bool>;