using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserHandler(
    IRepository<UserBasic> userRepository,
    ISecurityService securityService
    )
    : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var salt = securityService.GenerateSalt();
        var passwordWithSalt = securityService.HashPasswordWithSalt(command.Password,salt);
        var userBasic = new UserBasic(command.Account,command.Email,passwordWithSalt,salt);
        userBasic.CreateBy = userBasic.Id;
        userBasic.UpdateBy = userBasic.Id;
        userBasic.Activate();
        await userRepository.AddAsync(userBasic);
        return userBasic.Id;
    }
}