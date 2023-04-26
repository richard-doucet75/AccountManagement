using AccountServices.UseCases;
using AccountServices.UseCases.Models;
using AccountServicesApi.EndpointDefinitions.Presenters;
using AccountServicesApi.Utilities;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AccountServicesApi.EndpointDefinitions
{
    public class ChangePasswordEndpointDefinition : IEndpointDefinition
    {       
        public void DefineEndpoints(WebApplication app)
        {
            app.MapPost("api/Accounts/ChangePassword", ChangePassword)
                .WithTags("Account Management Endpoints")
                .WithSummary("Change Password")
                .WithDescription("Changes the current users password")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        public void DefineServices(IServiceCollection serviceCollection)
        {
        }

        private async Task ChangePassword(
            HttpContext httpConext, 
            IUserContext userContext, 
            ChangePassword changePassword, 
            ChangePasswordModel model)
        {
            var presenter = new ChangePasswordPresenter(httpConext.Response);
            await changePassword.Execute(
                presenter,
                userContext,
                model);
        }
    }
}
