namespace JackSite.Application.Features.Users.CreateUser;

public record CreateUserCommand(
    string UserName,
    string Email,
    string? Password
) : ICommand<CreateUserResponse>;

public record CreateUserResponse(
    long Id,
    string Password
);