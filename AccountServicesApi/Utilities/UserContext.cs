using AccountServices.UseCases.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;
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

    public static class UserContextConfigurator
    {
        public static void AddUserContext(this IServiceCollection serviceCollection)
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
    }
}
