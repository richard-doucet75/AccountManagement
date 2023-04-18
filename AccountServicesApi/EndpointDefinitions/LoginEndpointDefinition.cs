using System.Text;
using AccountServices.UseCases;
using AccountServicesApi.Utilities;
using AccountServices.UseCases.ValueTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AccountServicesApi.EndpointDefinitions.Presenters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace AccountServicesApi.EndpointDefinitions
{
    public class LoginEndpointDefinition : IEndpointDefinition
    {
        private const string JwtIssuerVariableName = "ACCOUNT_SERVICES_JWT_ISSUER";
        private const string JwtValidAudienceVariableName = "ACCOUNT_SERVICES_JWT_VALID_AUDIENCE";
        private const string JwtKeyVariableName = "ACCOUNT_SERVICES_JWT_KEY";
        public record JwtConfiguration(string Issuer, string ValidAudience, string Key);
        private static readonly JwtConfiguration Configuration;
        static LoginEndpointDefinition()
        {
            Configuration = new JwtConfiguration(
                Environment.GetEnvironmentVariable(JwtIssuerVariableName) 
                    ?? throw new EnvironmentException(JwtIssuerVariableName),
                Environment.GetEnvironmentVariable(JwtValidAudienceVariableName) 
                    ?? throw new EnvironmentException(JwtValidAudienceVariableName),
                Environment.GetEnvironmentVariable(JwtKeyVariableName) 
                    ?? throw new EnvironmentException(JwtKeyVariableName)
            );
        }

        [Serializable]
        public record LoginModel(
            [property: Required]
            EmailAddress EmailAddress,
            [property: Required]
            Password Password
        );

        public void DefineEndpoints(WebApplication app)
        {
            app.MapPost("api/Accounts/Login", Login)
                .WithTags("Account Management Endpoints")
                .WithSummary("Login to user account")
                .WithDescription("validates email address and password and generates Jwt token")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status401Unauthorized);

            app.UseAuthentication();
            app.UseAuthorization();
        }

        public void DefineServices(IServiceCollection serviceCollection)
        {

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration.Issuer,
                        ValidAudience = Configuration.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    Configuration.Key)),
                        ValidateIssuer = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true
                    };
                });
            serviceCollection.AddAuthorization();
        }

        [AllowAnonymous]
        private async Task Login(HttpContext context, Login login, LoginModel loginModel)
        {
            var presenter = new LoginJwtPresenter(context.Response, Configuration);
            await login.Execute(presenter, loginModel.EmailAddress, loginModel.Password);
        }
    }
}
