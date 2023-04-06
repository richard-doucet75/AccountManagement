using UseCases = AccountServices.UseCases;
using AccountServices.Infrastructure;
using AccountServicesApi.Utilities;
using AccountServicesApi.Presenters;
using System.ComponentModel.DataAnnotations;

namespace AccountServicesApi.EndpointDefinitions;

public class CreateAccountEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/", CreateAccount);
    }
    
    public void DefineServices(IServiceCollection serviceCollection)
    {
        serviceCollection.UseInfrastructure();
    }

    private static async Task<IResult> CreateAccount(
        UseCases.CreateAccount createAccount,
        [Required(AllowEmptyStrings = false)] string emailAddress,
        [Required(AllowEmptyStrings = false)] string password,
        [Required(AllowEmptyStrings = false)] string verifyPassword
        )
    {
        var presenter = new CreateAccountPresenter();
        await createAccount.Execute(presenter, emailAddress, password, verifyPassword);

        return presenter.Result;
    }
}