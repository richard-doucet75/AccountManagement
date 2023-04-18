using AccountServices.UseCases;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;
using AccountServicesApi.EndpointDefinitions.Presenters;
using AccountServicesApi.Utilities;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.ComponentModel.DataAnnotations;

namespace AccountServicesApi.EndpointDefinitions
{
    public class ChangePasswordEndpointDefinition : IEndpointDefinition
    {
        [Serializable]
        public record ChangePasswordModel(
            [property: Required]
            Password OldPassword,
            [property: Required] 
            Password NewPassword,
            [property: Required] 
            Password VerifyPasseword
        );
        
        public void DefineEndpoints(WebApplication app)
        {
            app.MapPost("api/Accounts/ChangePassword", ChangePassword)
                .WithTags("Account Management Endpoints")
                .WithSummary("Change Password")
                .WithDescription("Changes the current users password")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        public void DefineServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.TryAddScoped((services) =>
            {
                var context = services.GetRequiredService<IHttpContextAccessor>();
                if (context.HttpContext is null)
                    throw new Exception();

                return context.HttpContext.User;
            });

            serviceCollection.TryAddScoped<IUserContext, UserContext>();
        }

        private async Task ChangePassword(
            HttpContext httpConext, 
            IUserContext userContext, 
            ChangePassword changePassword, 
            ChangePasswordModel changePasswordModel)
        {
            var presenter = new ChangePasswordPresenter(httpConext.Response);
            await changePassword.Execute(
                presenter,
                userContext,
                changePasswordModel.OldPassword,
                changePasswordModel.NewPassword,
                changePasswordModel.VerifyPasseword);
        }
    }
}
