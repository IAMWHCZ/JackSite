/// <summary>
/// SendValidationCode 命令处理器
/// </summary>
public class SendValidationCodeHandler : ICommandHandler<SendValidationCodeCommand, bool>
{
    public async Task<bool> Handle(SendValidationCodeCommand command, CancellationToken cancellationToken)
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