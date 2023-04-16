using AccountServices.Infrastructure;
using AccountServicesApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using AccountServices.UseCases;
using AccountServices.UseCases.ValueTypes;
using System.ComponentModel.DataAnnotations;
using AccountServicesApi.EndpointDefinitions.Presenters;
using Microsoft.AspNetCore.Authorization;

namespace AccountServicesApi.EndpointDefinitions;

public class CreateAccountEndpointDefinition : IEndpointDefinition
{
    [Serializable]
    public record CreateAccountModel(
        [property:
            Required,
            MinLength(EmailAddress.MinimumLength),
            MaxLength(EmailAddress.MaximumLenght)]

        EmailAddress EmailAddress,
        [property: 
            Required,
            MinLength(Password.MinimumLength), 
            MaxLength(Password.MaximumLength)] 
        Password Password,
        [property: 
            Required,
            MinLength(Password.MinimumLength),
            MaxLength(Password.MaximumLength)] 
            Password VerifyPassword
        );

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
            [FromBody] CreateAccountModel model
        )
    {
        
        var presenter = new CreateAccountPresenter(httpContext.Response);
        await createAccount.Execute(presenter, model.EmailAddress, model.Password, model.VerifyPassword);
    }
}