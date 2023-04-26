using Microsoft.Extensions.DependencyInjection;

using AccountServices.Gateways;
using AccountServices.Infrastructure.Gateways;
using AccountServices.UseCases.Services;


namespace AccountServices.Infrastructure
{
    public static class Installer
    {
        public static void UseInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAccountGateway, AccountGateway>();
            serviceCollection.AddScoped<UseCases.CreateAccount>();
            serviceCollection.AddScoped<UseCases.Login>();
            serviceCollection.AddScoped<UseCases.ChangePassword>();
            serviceCollection.AddScoped<UseCases.GetAccount>();
        }
    }
}
