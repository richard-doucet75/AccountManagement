using AccountServicesApi.Utilities;
using AccountServicesApi.ValueTypeConverters;

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