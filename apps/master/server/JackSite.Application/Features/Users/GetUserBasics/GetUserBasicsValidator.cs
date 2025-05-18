using FluentValidation;
/// <summary>
/// GetUserBasics 命令验证器
/// </summary>
public class GetUsersValidator : AbstractValidator<GetUserBasicsCommand>
{
    public GetUsersValidator()
    {
    }
}
