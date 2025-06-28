using JackSite.Authentication.Application.Features.Emails;
using JackSite.Authentication.Application.Features.Emails.Queries.VerifyCode;
using JackSite.Authentication.Application.Features.Users.Queries.UserIsExist;
using JackSite.Authentication.Enums.Emails;

namespace JackSite.Authentication.Application.Features.Users.Commands.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator(ISender sender)
    {
        RuleFor(x => x.Account)
            .NotNull()
            .NotEmpty()
            .WithMessage("Account is required.")
            .MinimumLength(6)
            .MaximumLength(16)
            .WithMessage("Account must be between 6 and 16 characters long.")
            .MustAsync(async (account, cancel) =>
            {
                var isExist = await sender.Send(new UserIsExistQuery(account), cancel);
                return !isExist;
            })
            .WithMessage("Account already exists.");

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .MaximumLength(16)
            .WithMessage("Password must be between 6 and 16 characters long.")
            .Equal(x => x.ConfirmPassword)
            .WithMessage("Password and confirm password do not match.");

        RuleFor(x => x.ConfirmPassword)
            .NotNull()
            .NotEmpty()
            .WithMessage("Confirm password is required.")
            .MinimumLength(6)
            .MaximumLength(16)
            .WithMessage("Confirm password must be between 6 and 16 characters long.");

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MustAsync(async (email, cancel) =>
            {
                var isExist = await sender.Send(new EmailIsBindQuery(email), cancel);
                return !isExist;
            })
            .WithMessage("Email already exists.")
            .MustAsync(async (command, _, cancel) =>
            {
                var isVerify = await sender.Send(new VerifyCodeQuery(
                        command.Email,
                        SendEmailType.RegisterUser,
                        command.ValidationCode)
                    , cancel);
                return isVerify;
            })
            .WithMessage("Invalid or expired verification code.");
    }
}