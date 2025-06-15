namespace JackSite.Authentication.Application.Features.Users.Queries.UserIsExist;

/// <summary>
/// UserIsExist 命令验证器
/// </summary>
public sealed class UserIsExistValidator : AbstractValidator<UserIsExistQuery>
{
    public UserIsExistValidator()
    {
        RuleFor(x => x.Username
            )
            .NotEmpty()
            .WithMessage("Username is required")
            .NotNull()
            .WithMessage("Username is required")
            .MaximumLength(20)
            .WithMessage("Username must be less than 20 characters")
            .MinimumLength(6)
            .WithMessage("Username must be at least 6 characters long");
    }
}