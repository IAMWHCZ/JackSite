using FluentValidation;
/// <summary>
/// GetUsers 命令验证器
/// </summary>
public class GetUsersValidator : AbstractValidator<GetUsersCommand>
{
    public GetUsersValidator()
    {
    }
}
