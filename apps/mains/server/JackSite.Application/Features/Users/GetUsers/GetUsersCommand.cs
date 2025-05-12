using JackSite.Application.CQRS.Commands;
/// <summary>
/// GetUsers 命令
/// </summary>
public record GetUsersCommand() : ICommand<bool>
{
}