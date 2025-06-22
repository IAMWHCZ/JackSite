using JackSite.Authentication.Application.Features.Users.Queries.UserIsExist;
using JackSite.Authentication.WebAPI.Modules;
using JackSite.Shared.Results;
using MediatR;

namespace JackSite.Authentication.WebAPI.Endpoints.Users;

public sealed class UserBasicEndpoint:IApiModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/exists/{username}", async (string username, ISender sender) =>
        {
            var exists = await sender.Send(new UserIsExistQuery(username));
            return exists ? ApiResult.Fail("Username already exists") : ApiResult<bool>.Ok(exists, "Username is available");
        })
        .WithName("CheckUsernameExists")
        .WithSummary("Check if a username already exists")
        .WithTags("Users")
        .Produces<ApiResult<bool>>()
        .Produces<ApiResult>(400)
        .Produces<ApiResult>(500);
    }
}