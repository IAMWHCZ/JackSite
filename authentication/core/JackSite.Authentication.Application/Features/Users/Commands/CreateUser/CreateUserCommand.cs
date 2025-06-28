namespace JackSite.Authentication.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Account,
    string Password,
    string ConfirmPassword,
    string Email,
    string ValidationCode
):ICommand<Guid>;