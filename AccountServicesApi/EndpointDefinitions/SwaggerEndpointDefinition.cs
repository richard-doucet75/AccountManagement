using AccountServices.UseCases.ValueTypes;
using AccountServicesApi.Utilities;
using AccountServicesApi.ValueTypeConverters;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using static AccountServicesApi.EndpointDefinitions.CreateAccountEndpointDefinition;

namespace AccountServicesApi.EndpointDefinitions;

public class SwaggerEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen(
                options => options.ConfigureSwaggerGen()
            );
    }

    public void DefineEndpoints(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}