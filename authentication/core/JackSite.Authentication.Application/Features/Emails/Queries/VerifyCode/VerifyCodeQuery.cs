using JackSite.Authentication.Enums.Emails;

namespace JackSite.Authentication.Application.Features.Emails.Queries.VerifyCode;

/// <summary>
/// VerifyCode 命令
/// </summary>
public record VerifyCodeQuery(
    string Email,
    SendEmailType Type,
    string Code
) : IQuery<bool>;