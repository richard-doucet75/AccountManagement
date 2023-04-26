using Microsoft.AspNetCore.Authorization;

using AccountServicesApi.EndpointDefinitions.Presenters;
using AccountServicesApi.Utilities;

using AccountServices.Infrastructure;

using AccountServices.UseCases;
using AccountServices.UseCases.Models;

namespace AccountServicesApi.EndpointDefinitions;

public class CreateAccountEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("api/Accounts/", CreateAccount)
            .WithTags("Account Management Endpoints")
            .WithSummary("Create a user account")
            .WithDescription("Create a user account given an email address and matching password and verify password")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    public void DefineServices(IServiceCollection serviceCollection)
    {
    }

    [AllowAnonymous]
    private static async Task CreateAccount(
            HttpContext httpContext,
            LinkGenerator linker,
            CreateAccount createAccount,
            CreateAccountModel model
        )
    {
        var presenter = new CreateAccountPresenter(httpContext.Response, linker);
        await createAccount.Execute(presenter, model);
    }
}