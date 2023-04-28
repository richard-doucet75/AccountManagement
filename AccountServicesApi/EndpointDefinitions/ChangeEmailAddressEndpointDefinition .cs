using AccountServices.UseCases;
using AccountServices.UseCases.Models;
using AccountServicesApi.EndpointDefinitions.Presenters;
using AccountServicesApi.Utilities;

namespace AccountServicesApi.EndpointDefinitions
{
    public class ChangeEmailAddressEndpointDefinition : IEndpointDefinition
    {       
        public void DefineEndpoints(WebApplication app)
        {
            app.MapPost("api/Accounts/ChangeEmailAddress", ChangeEmailAddress)
                .WithTags("Account Management Endpoints")
                .WithSummary("Change Email address")
                .WithDescription("Changes the emailAddress for this account")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        public void DefineServices(IServiceCollection serviceCollection)
        {
        }

        private async Task ChangeEmailAddress(
            HttpContext httpConext, 
            IUserContext userContext, 
            ChangeEmailAddress changeEmailAddress, 
            ChangeEmailAddressModel model)
        {
            var presenter = new ChangeEmailAddressPresenter(httpConext.Response);
            await changeEmailAddress.Execute(
                presenter,
                userContext,
                model);
        }
    }
}
