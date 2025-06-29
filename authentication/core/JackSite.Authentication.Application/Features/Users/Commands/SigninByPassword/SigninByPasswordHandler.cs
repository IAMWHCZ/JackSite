/// <summary>
/// SigninByPassword 命令处理器
/// </summary>
public class SigninByPasswordHandler : ICommandHandler<SigninByPasswordCommand, bool>
{
    public async Task<bool> Handle(SigninByPasswordCommand command, CancellationToken cancellationToken)
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