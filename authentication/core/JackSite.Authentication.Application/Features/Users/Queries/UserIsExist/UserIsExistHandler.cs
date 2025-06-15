/// <summary>
/// UserIsExist 命令处理器
/// </summary>
public class UserIsExistHandler : IQueryHandler<UserIsExistQuery, bool>
{
    public async Task<bool> Handle(UserIsExistQuery query, CancellationToken cancellationToken)
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