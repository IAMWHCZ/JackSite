using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Application.Exceptions.User;
using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Application.Features.Users.Queries.UserIsExist;

/// <summary>
/// UserIsExist 命令处理器
/// </summary>
public sealed class UserIsExistHandler(
    IRepository<UserBasic> userRepository
)
    : IQueryHandler<UserIsExistQuery, bool>
{
    public async Task<bool> Handle(UserIsExistQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var userIsExist = await userRepository.ExistsAsync(x=>x.Username == query.Username);
            return userIsExist;
        }
        catch (Exception ex)
        {
            throw new UserException(query.Username,ex);
        }
    }
}