namespace JackSite.Authentication.Application.Features.Emails;

/// <summary>
/// SendValidationCode 命令验证器
/// </summary>
public class SendValidationCodeValidator : AbstractValidator<SendValidationCodeCommand>
{
    public SendValidationCodeValidator(ICacheService cache)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email is required.");
        
        RuleFor(x => x.Type)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email type is required.")
            .IsInEnum()
            .WithMessage("Invalid email type.")
            .MustAsync(async (command, _, _, _) => 
            {
                var key = cache.BuildCacheKey(command.Email, command.Type);
                var exists = await cache.ExistsAsync(key);
                return !exists; 
            })
            .WithMessage("This email type has already been sent a validation code recently.");
        


    }
}