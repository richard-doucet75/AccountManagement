using AccountServices.Infrastructure;
using AccountServicesApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using AccountServices.UseCases;
using AccountServices.UseCases.ValueTypes;
using System.ComponentModel.DataAnnotations;
using AccountServicesApi.EndpointDefinitions.Presenters;
using Microsoft.AspNetCore.Authorization;
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
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    public void DefineServices(IServiceCollection serviceCollection)
    {
        serviceCollection.UseInfrastructure();
    }

    [AllowAnonymous]
    private static async Task CreateAccount(
            HttpContext httpContext,
            CreateAccount createAccount,
            CreateAccountModel model
        )
    {
        
        var presenter = new CreateAccountPresenter(httpContext.Response);
        await createAccount.Execute(presenter, model);
    }
}