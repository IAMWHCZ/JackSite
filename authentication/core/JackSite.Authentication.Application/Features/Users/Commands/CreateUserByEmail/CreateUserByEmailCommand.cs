namespace JackSite.Authentication.Application.Features.Users.Commands.CreateUserByEmail;


public record CreateUserByEmailCommand(
    string Email,
    int Code,
    string Password) : ICommand<Guid>;
