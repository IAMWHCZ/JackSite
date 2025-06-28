namespace JackSite.Authentication.Application.Features.Emails.Queries.VerifyCode;

/// <summary>
/// VerifyCode 命令处理器
/// </summary>
public class VerifyCodeHandler(ICacheService cache): IQueryHandler<VerifyCodeQuery, bool>
{
    public async Task<bool> Handle(VerifyCodeQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var key = cache.BuildCacheKey(query.Email, query.Type);
            var code = await cache.GetAsync<string>(key);
                
            return query.ValidationCode == code;
        }
        catch (Exception ex)
        {
            throw new EmailException(ex.Message, ex);
        }
    }
}