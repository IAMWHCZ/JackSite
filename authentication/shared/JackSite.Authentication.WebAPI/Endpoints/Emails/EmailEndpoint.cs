using JackSite.Authentication.Application.Features.Users.Queries.EmailIsBind;


namespace JackSite.Authentication.WebAPI.Endpoints.Emails;

public class EmailEndpoint:IApiModule
{
    public void AddRoutes(IEndpointRouteBuilder routeGroup)
    {
        routeGroup.MapGet("/email/is-bind", async (string email,ISender sender) =>
        {
            var result = await sender.Send(new EmailIsBindQuery(email));
            return result ? Results.Ok() : Results.BadRequest("Invalid email confirmation token.");
        })
        .WithName("Check Email is Bind")
        .WithSummary("Check if an email is already bound to a user account")
        .WithDescription("This endpoint checks if the provided email is already bound to a user account. If the email is bound, it returns a 200 OK response; otherwise, it returns a 400 Bad Request response.") 
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}