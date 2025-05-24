namespace JackSite.Infrastructure.Services;

public class UserService(
    IUserBasicRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ICacheService cacheService,
    IEmailService emailService,
    IEmailTemplateService emailTemplateService,
    IRequestHeaderService requestHeaderService
)
    : IUserService
{
    public async Task<UserBasic?> AuthenticateAsync(string username, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUsernameAsync(username, cancellationToken);

        if (user is not { IsActive: true })
        {
            return null;
        }

        // 使用新的密码哈希器验证密码
        var isPasswordValid = passwordHasher.VerifyPassword(
            password,
            user.PasswordHash,
            user.Salt);

        return isPasswordValid ? user : null;
    }

    public async Task<UserBasic> RegisterAsync(string username, string email, string password,
        CancellationToken cancellationToken = default)
    {
        // 检查用户名和邮箱是否已存在
        var existingUser = await userRepository.GetByUsernameAsync(username, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Username '{username}' is already taken.");
        }

        existingUser = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Email '{email}' is already registered.");
        }

        // 使用新的密码哈希器创建密码哈希
        var hashResult = passwordHasher.HashPassword(password);

        // 使用领域构造函数创建用户实体
        var user = new UserBasic(username, email, hashResult.Hash, hashResult.Salt);

        // 使用工作单元模式保存
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return user;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task SendVerificationEmailAsync(UserBasic user, SendEmailType type,
        CancellationToken cancellationToken = default)
    {
        var @params = requestHeaderService.HeaderParams;

        var key = $"{user.Email}-{type}";

        var code = await cacheService.GetOrCreateAsync(key,
            () => Task.FromResult(VerificationCodeGenerator.GenerateNumeric()),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(30),
            cancellationToken);

        var message = await emailTemplateService.GetEmailTemplateAsync(type.ToEmailTemplateType(),
            @params.Language.ToString(), new Dictionary<string, string>
            {
                { "VerificationCode", code }
            });

        await emailService.SendHtmlEmailAsync(user.Email, message, type);
    }

    public async Task<bool> VerifyEmailAsync(string email, string code, SendEmailType type,
        CancellationToken cancellationToken = default)
    {
        var key = $"{email}-{type}";
        var exists = await cacheService.ExistsAsync(key, cancellationToken);
        if (!exists) return exists;
        var token = await cacheService.GetAsync<string>(email, cancellationToken);
        return token == code;
    }


    public async Task<bool> AssignRoleToUserAsync(long userId, long roleId,
        CancellationToken cancellationToken = default)
    {
        // 检查用户和角色是否存在
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return false;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}