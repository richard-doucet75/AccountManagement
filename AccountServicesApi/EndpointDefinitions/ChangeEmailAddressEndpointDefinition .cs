using AccountServices.UseCases;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;
using AccountServicesApi.EndpointDefinitions.Presenters;
using AccountServicesApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;

namespace AccountServicesApi.EndpointDefinitions
{
    public class ChangeEmailAddressEndpointDefinition : IEndpointDefinition
    {       
        public void DefineEndpoints(WebApplication app)
        {
            app.MapPost("api/Accounts/{accountId}/ChangeEmailAddress", ChangeEmailAddress)
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

        [Authorize]
        private async Task ChangeEmailAddress(
            HttpContext httpConext, 
            IUserContext userContext, 
            ChangeEmailAddress changeEmailAddress, 
            Guid accountId, 
            EmailAddress newEmailAddress)
        {
            var presenter = new ChangeEmailAddressPresenter(httpConext.Response);
            await changeEmailAddress.Execute(
                presenter,
                userContext,
                accountId,
                newEmailAddress);
        }
    }
}
