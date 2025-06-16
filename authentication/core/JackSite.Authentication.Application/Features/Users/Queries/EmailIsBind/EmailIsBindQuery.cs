namespace JackSite.Authentication.Application.Features.Users.Queries.UserIsExist;

/// <summary>
/// UserIsExist 命令
/// </summary>
public sealed record UserIsExistQuery(string Username) : IQuery<bool>;