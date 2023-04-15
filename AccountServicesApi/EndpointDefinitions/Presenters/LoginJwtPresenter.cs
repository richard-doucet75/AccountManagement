using AccountServices.UseCases.ValueTypes;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using static AccountServices.UseCases.Login;
using static AccountServicesApi.EndpointDefinitions.JwtEndpointDefinition;

namespace AccountServicesApi.EndpointDefinitions.Presenters
{
    public class LoginJwtPresenter : IPresenter
    {
        private JwtConfiguration Configuration { get; }

        public IResult? Result { get; set; }
        

        public LoginJwtPresenter(JwtConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task PresentAccessDenied()
        {
            Result = await Task.FromResult(Results.Unauthorized());
        }

        public async Task PresentNotFound()
        {
            Result = await Task.FromResult(Results.NotFound());
        }

        public async Task PresentSuccess(Guid accountId, EmailAddress emailAddress)
        {
            await Task.Run(() =>
            {
                var issuer = Configuration.Issuer;
                var audience = Configuration.ValidAudience;
                var key = Encoding.ASCII.GetBytes(Configuration.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", accountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, emailAddress),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);
                Result = Results.Ok(stringToken);
            });
            
        }
    }
}
