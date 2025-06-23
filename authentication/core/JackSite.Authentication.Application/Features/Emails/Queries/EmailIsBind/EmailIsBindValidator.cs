namespace JackSite.Authentication.Application.Features.Emails;

/// <summary>
/// UserIsExist 命令验证器
/// </summary>
public sealed class EmailIsBindValidator : AbstractValidator<EmailIsBindQuery>
{
    public EmailIsBindValidator()
    {
        RuleFor(x => x.Email
            )
            .NotEmpty()
            .WithMessage("Email is required")
            .NotNull()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format.");
    }
}