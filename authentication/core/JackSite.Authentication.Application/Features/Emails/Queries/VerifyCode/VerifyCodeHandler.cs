/// <summary>
/// VerifyCode 命令处理器
/// </summary>
public class VerifyCodeHandler : IQueryHandler<VerifyCodeQuery, bool>
{
    public async Task<bool> Handle(VerifyCodeQuery query, CancellationToken cancellationToken)
    {
        try
        {
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}