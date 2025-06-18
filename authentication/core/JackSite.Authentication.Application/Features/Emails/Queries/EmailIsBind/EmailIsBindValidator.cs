namespace JackSite.Authentication.Application.Features.Users.Queries.EmailIsBind;

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
            .WithMessage("Email is not valid") ;
    }
}