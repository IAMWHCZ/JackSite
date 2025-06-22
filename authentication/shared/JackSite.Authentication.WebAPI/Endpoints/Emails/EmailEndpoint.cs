using JackSite.Authentication.Application.Features.Emails.Queries.VerifyCode;
using JackSite.Shared.Results;


namespace JackSite.Authentication.WebAPI.Endpoints.Emails;

public class EmailEndpoint:IApiModule
{
    public void AddRoutes(IEndpointRouteBuilder routeGroup)
    {
        routeGroup.MapGet("/is-bind", async (string email,ISender sender) =>
        {
            var result = await sender.Send(new EmailIsBindQuery(email));
            return !result ? ApiResult.Ok(result) : ApiResult.Fail();
        })
        .WithTags("Email")
        .WithDisplayName("EmailIsBind")
        .WithName("Check Email is Bind")
        .WithSummary("Check if an email is already bound to a user account")
        .WithDescription("This endpoint checks if the provided email is already bound to a user account. If the email is bound, it returns a 200 OK response; otherwise, it returns a 400 Bad Request response.") 
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        routeGroup.MapPost("send-validation-code",async (SendValidationCodeCommand command, ISender sender) =>
        {
            var send = await sender.Send(command);
            return ApiResult.Ok(send,"Validation code sent successfully");
        })
        .WithTags("Email")
        .WithDisplayName("SendValidationCode")
        .WithName("Send Email Validation Code")
        .WithSummary("Send a validation code to the specified email address")
        .WithDescription("This endpoint sends a validation code to the specified email address. If the email is valid and the code is sent successfully, it returns a 200 OK response; otherwise, it returns a 400 Bad Request response.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
        
        routeGroup.MapPost("verify-code", async (VerifyCodeQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return ApiResult.Ok(result,"Validation code verified successfully");
        })
        .WithTags("Email")
        .WithDisplayName("VerifyCode")
        .WithName("Verify Email Code")
        .WithSummary("Verify the validation code sent to the email address")
        .WithDescription("This endpoint verifies the validation code sent to the specified email address. If the code is valid, it returns a 200 OK response; otherwise, it returns a 400 Bad Request response.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}