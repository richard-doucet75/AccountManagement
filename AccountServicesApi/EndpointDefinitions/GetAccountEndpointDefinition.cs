using AccountServices.UseCases;
using AccountServices.UseCases.Models;
using AccountServicesApi.EndpointDefinitions.Presenters;
using AccountServicesApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.SqlServer.Server;

namespace AccountServicesApi.EndpointDefinitions;

public class GetAccountEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection serviceCollection)
    {
    }


    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("api/Accounts/{accountId}", GetAccount)
            .WithName("GetAccount")
            .WithTags("Account Management Endpoints")
            .WithSummary("Login to user account")
            .WithDescription("validates email address and password and generates Jwt token")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    [Authorize]
    private async Task GetAccount(
        HttpContext context, 
        IUserContext userContext, 
        GetAccount getAccount,
        Guid accountId)
    {
        var presenter = new GetAccountPresenter(context.Response);
        await getAccount.Execute(presenter, userContext, accountId);
    }
}