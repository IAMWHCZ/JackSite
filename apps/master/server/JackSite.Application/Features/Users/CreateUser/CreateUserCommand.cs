

namespace JackSite.Application.Features.Users.CreateUser;

public record CreateUserCommand(
    string UserName,
    string Email,
    string? Password
    ):ICommand;