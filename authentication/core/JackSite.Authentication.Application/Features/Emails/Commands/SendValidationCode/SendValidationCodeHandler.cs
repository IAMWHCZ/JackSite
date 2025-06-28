namespace JackSite.Authentication.Application.Features.Emails;

/// <summary>
/// SendValidationCode 命令处理器
/// </summary>
public class SendValidationCodeHandler(
    ICacheService cacheService,
    IEmailService emailService,
    IAccessBaseService accessBaseService
    ): ICommandHandler<SendValidationCodeCommand, bool>
{
    public async Task<bool> Handle(
        SendValidationCodeCommand command, 
        CancellationToken cancellationToken)
    {
        try
        {
            var currentFormBase = accessBaseService.GetCurrentFormBase();
            
            // 生成验证码
            var code = Random.Shared.Next(100000, 999999).ToString();
            var key = cacheService.BuildCacheKey(command.Email, command.Type);
            // 将验证码存入缓存
            await emailService.SendEmailWithTemplateAsync(
                command.Email,
                EmailTemplateConst.GetTemplateNameByType(
                    command.Type, 
                    !currentFormBase.Language.StartsWith("zh", StringComparison.OrdinalIgnoreCase
                    )),
                command.Type,
                new Dictionary<string, string>
                {
                    {"Email",command.Email},
                    { "Code", code }
                }
            );
            await cacheService.SetAsync(key, code,
                TimeSpan.FromMinutes(5));
            return true;
        }
        catch (Exception ex)
        {
            throw new EmailException(ex.Message, ex);
        }
    }
}