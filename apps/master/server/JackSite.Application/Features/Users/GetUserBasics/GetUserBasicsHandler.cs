using JackSite.Application.CQRS.Commands;
using JackSite.Domain.Services;

/// <summary>
/// GetUserBasics 命令处理器
/// </summary>
public class GetUsersHandler : ICommandHandler<GetUserBasicsCommand, bool>
{
    private readonly ILogService _logger;

    public GetUsersHandler(ILogService logger)
    {
        _logger = logger.ForContext<GetUsersHandler>();
    }

    public async Task<bool> Handle(GetUserBasicsCommand command, CancellationToken cancellationToken)
    {
        try
        {
          
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "GetUserBasics 命令处理失败");
            return false;
        }
    }
}