using JackSite.Authentication.WebAPI.Modules;

namespace JackSite.Authentication.WebAPI.Endpoints.Users;

public class EmailEndpoint:IApiModule
{
    public void AddRoutes(IEndpointRouteBuilder routeGroup)
    {
        routeGroup.MapGet("/users/email/confirm", async (string email, string token, IEmailService emailService) =>
        {
            var result = await emailService.ConfirmEmailAsync(email, token);
            return result ? Results.Ok() : Results.BadRequest("Invalid email confirmation token.");
        })
        .WithName("ConfirmEmail")
        .WithSummary("Confirm user email address")
        .WithDescription("Confirms a user's email address using a confirmation token sent to the user's email.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}