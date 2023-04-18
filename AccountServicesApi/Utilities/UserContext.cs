using AccountServices.UseCases.Models;
using System.Security.Claims;

namespace AccountServicesApi.Utilities
{
    public class UserContext : IUserContext
    {
        public UserContext(ClaimsPrincipal user) 
        {
            if (Guid.TryParse(user.Claims.SingleOrDefault(c => c.Type == "Id")?.Value, out var result))
            {
                AccountId = result;
            }
        }
        
        public Guid? AccountId { get; }
    }
}
