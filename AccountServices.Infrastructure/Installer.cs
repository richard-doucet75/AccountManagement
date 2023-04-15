using Microsoft.Extensions.DependencyInjection;

using AccountServices.Gateways;
using AccountServices.Services;
using AccountServices.Infrastructure.Gateways;
using AccountServices.Infrastructure.Services;


namespace AccountServices.Infrastructure
{
    public static class Installer
    {
        public static void UseInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAccountGateway, AccountGateway>();
            serviceCollection.AddScoped<IPasswordHasher, PasswordHasher>();
            serviceCollection.AddScoped<UseCases.CreateAccount>();
            serviceCollection.AddScoped<UseCases.Login>();
        }
    }
}
