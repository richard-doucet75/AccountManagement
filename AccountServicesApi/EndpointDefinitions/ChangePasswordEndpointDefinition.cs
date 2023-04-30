using AccountServices.UseCases;
using AccountServices.UseCases.Models;
using AccountServicesApi.EndpointDefinitions.Presenters;
using AccountServicesApi.Utilities;

namespace AccountServicesApi.EndpointDefinitions
{
    public class ChangePasswordEndpointDefinition : IEndpointDefinition
    {       
        public void DefineEndpoints(WebApplication app)
        {
            app.MapPost("api/Accounts/{accountId}/ChangePassword", ChangePassword)
                .WithTags("Account Management Endpoints")
                .WithSummary("Change Password")
                .WithDescription("Changes the current users password")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        public void DefineServices(IServiceCollection serviceCollection)
        {
        }

        private async Task ChangePassword(
            ChangePassword changePassword,
            HttpContext httpConext, 
            IUserContext userContext, 
            Guid accountId,
            ChangePasswordModel model)
        {
            var presenter = new ChangePasswordPresenter(httpConext.Response);
            await changePassword.Execute(
                presenter,
                userContext,
                accountId,
                model);
        }
    }
}
