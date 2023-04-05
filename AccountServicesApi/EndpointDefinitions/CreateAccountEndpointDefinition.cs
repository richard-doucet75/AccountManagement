using UseCases = AccountServices.UseCases;
using AccountServices.Infrastructure;
using AccountServicesApi.Utilities;
using AccountServicesApi.Presenters;


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

    private static async Task<IResult> CreateAccount(UseCases.CreateAccount createAccount, CreateAccountPresenter.Request request)
    {
        var presenter = new CreateAccountPresenter();
        await createAccount.Execute(presenter, request.EmailAddress, request.Password, request.VerifyPassword);

        return presenter.Result;
    }
}