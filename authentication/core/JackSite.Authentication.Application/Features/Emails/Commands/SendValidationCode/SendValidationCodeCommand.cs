using JackSite.Authentication.Enums.Emails;

namespace JackSite.Authentication.Application.Features.Emails;

/// <summary>
/// SendValidationCode 命令
/// </summary>
public record SendValidationCodeCommand(
    string Email,
    SendEmailType Type
) : ICommand<bool>;