using AccountServices.UseCases.ValueTypes;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using static AccountServices.UseCases.Login;
using static AccountServicesApi.EndpointDefinitions.LoginEndpointDefinition;
using static System.Net.Mime.MediaTypeNames;

namespace AccountServicesApi.EndpointDefinitions.Presenters
{
    public class LoginJwtPresenter : IPresenter
    {
        private readonly JwtConfiguration _configuration;
        private readonly HttpResponse _response;

       

        public LoginJwtPresenter(HttpResponse response, JwtConfiguration configuration)
        {
            _configuration = configuration;
            _response = response;
        }

        public async Task PresentAccessDenied()
        {
            _response.StatusCode = StatusCodes.Status401Unauthorized;
            await Task.CompletedTask;
        }

        public async Task PresentNotFound()
        {
            _response.StatusCode = StatusCodes.Status404NotFound;
            await Task.CompletedTask;
        }

        public async Task PresentSuccess(Guid accountId, EmailAddress emailAddress)
        {
            var issuer = _configuration.Issuer;
            var audience = _configuration.ValidAudience;
            var key = Encoding.ASCII.GetBytes(_configuration.Key);
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
            _response.StatusCode = StatusCodes.Status200OK;
            _response.ContentType = Text.Plain;

            await _response.WriteAsync(stringToken);
        }
    }
}
