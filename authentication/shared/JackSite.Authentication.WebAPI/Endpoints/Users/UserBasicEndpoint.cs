using JackSite.Authentication.Application.Features.Users.Commands.CreateUser;
using JackSite.Authentication.Application.Features.Users.Queries.UserIsExist;
using JackSite.Authentication.WebAPI.Modules;
using JackSite.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JackSite.Authentication.WebAPI.Endpoints.Users;

public sealed class UserBasicEndpoint : IApiModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/exists/{username}", async (string username, ISender sender) =>
            {
                var exists = await sender.Send(new UserIsExistQuery(username));
                return exists
                    ? ApiResult.Fail("Username already exists")
                    : ApiResult<bool>.Ok(exists, "Username is available");
            })
            .WithName("CheckUsernameExists")
            .WithSummary("Check if a username already exists")
            .WithTags("Users")
            .Produces<ApiResult<bool>>()
            .Produces<ApiResult>(400)
            .Produces<ApiResult>(500);


        app.MapPost("register", async ([FromBody] CreateUserCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result > Guid.Empty
                    ? ApiResult.Ok(result, "User created successfully")
                    : ApiResult.Fail("Failed to create user");
            })
            .WithName("RegisterUser")
            .WithSummary("Register a new user")
            .WithTags("Users")
            .Produces<ApiResult<Guid>>()
            .Produces<ApiResult>(400)
            .Produces<ApiResult>(500);
    }
}