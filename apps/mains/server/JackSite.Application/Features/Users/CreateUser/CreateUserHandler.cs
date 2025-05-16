using System.Security.Cryptography;
using System.Text;
using JackSite.Domain.Repositories;

namespace JackSite.Application.Features.Users.CreateUser;

public class CreateUserHandler(IUserBasicRepository userBasicRepository):ICommandHandler<CreateUserCommand>
{
    public async Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var password = string.IsNullOrEmpty(command.Password) ? Generate(14) : command.Password;

        return Unit.Value;
    }
    
     private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string DigitChars = "0123456789";
    private const string SpecialChars = "!@#$%^&*(),.?\":{}|<>";

    // 生成随机密码
    private static string Generate(int length = 12)
    {
        if (length < 8 || length > 20)
        {
            throw new ArgumentException("密码长度必须在 8 到 20 个字符之间。");
        }

        var passwordBuilder = new StringBuilder();
        var randomBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();

        // 确保密码包含至少一个大写字母、小写字母、数字和特殊字符
        passwordBuilder.Append(GetRandomChar(UppercaseChars, rng));
        passwordBuilder.Append(GetRandomChar(LowercaseChars, rng));
        passwordBuilder.Append(GetRandomChar(DigitChars, rng));
        passwordBuilder.Append(GetRandomChar(SpecialChars, rng));

        // 填充剩余字符
        rng.GetBytes(randomBytes);
        for (int i = 4; i < length; i++)
        {
            char randomChar = GetRandomChar(UppercaseChars + LowercaseChars + DigitChars + SpecialChars, rng);
            passwordBuilder.Append(randomChar);
        }

        // 打乱字符顺序
        var passwordArray = passwordBuilder.ToString().ToCharArray();
        Shuffle(passwordArray, rng);

        return new string(passwordArray);
    }

    // 获取随机字符
    private static char GetRandomChar(string chars, RandomNumberGenerator rng)
    {
        var randomByte = new byte[1];
        do
        {
            rng.GetBytes(randomByte);
        } while (randomByte[0] >= (256 - (256 % chars.Length)));

        return chars[randomByte[0] % chars.Length];
    }

    // Fisher-Yates 洗牌算法
    private static void Shuffle(char[] array, RandomNumberGenerator rng)
    {
        var randomBytes = new byte[array.Length];
        rng.GetBytes(randomBytes);

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = randomBytes[i] % (i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

}