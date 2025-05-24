using JackSite.Application.CQRS.Commands;
/// <summary>
/// GetUserBasics 命令
/// </summary>
public record GetUserBasicsCommand() : ICommand<bool>
{
}