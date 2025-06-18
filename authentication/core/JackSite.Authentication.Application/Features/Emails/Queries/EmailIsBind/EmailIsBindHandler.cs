using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Application.Exceptions.User;
using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Application.Features.Emails.Queries.EmailIsBind;

/// <summary>
/// UserIsExist 命令处理器
/// </summary>
public sealed class EmailIsBindHandler(
    IRepository<UserBasic> userRepository
)
    : IQueryHandler<EmailIsBindQuery, bool>
{
    public async Task<bool> Handle(EmailIsBindQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var emailIsBind = await userRepository.ExistsAsync(x=>x.Email == query.Email && !x.EmailConfirmed);
            return emailIsBind;
        }
        catch (Exception ex)
        {
            throw new UserException(query.Email,ex);
        }
    }
}