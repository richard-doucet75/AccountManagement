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
        app.MapPost("/", CreateAccount)
            .WithTags("Account Management Endpoints")
            .WithSummary("Create a user account")
            .WithDescription("Create a user account given an email address and matching password and verify password")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public void DefineServices(IServiceCollection serviceCollection)
    {
        serviceCollection.UseInfrastructure();
    }

    [AllowAnonymous]
    private static async Task<IResult> CreateAccount(
            
            CreateAccount createAccount,
            [FromBody] CreateAccountModel model
        )
    {
        
        var presenter = new CreateAccountPresenter();
        await createAccount.Execute(presenter, model.EmailAddress, model.Password, model.VerifyPassword);

        return presenter.Result;
    }
}