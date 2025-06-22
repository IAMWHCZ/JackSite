namespace JackSite.Authentication.Application.Features.Emails.Queries.VerifyCode;

/// <summary>
/// VerifyCode 命令验证器
/// </summary>
public class VerifyCodeValidator : AbstractValidator<VerifyCodeQuery>
{
    public VerifyCodeValidator(ICacheService cache)
    {
        RuleFor(x=>x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email is required.");
        
        RuleFor(x=>x.Type)
            .NotNull()
            .NotEmpty()
            .WithMessage("Type is required.")
            .IsInEnum()
            .WithMessage("Invalid type value.");

        RuleFor(x => x.Code)
            .NotNull()
            .NotEmpty()
            .WithMessage("Code is required.") 
            .MustAsync(async (c, _, _, _) =>
            {
                var key = cache.BuildCacheKey(c.Email, c.Type);
                var exists = await cache.ExistsAsync(key);
                return !exists;
            })
            .WithMessage("Code does not exist or has expired.");


    }
}