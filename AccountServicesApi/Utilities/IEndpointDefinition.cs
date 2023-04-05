namespace AccountServicesApi.Utilities
{
    public interface IEndpointDefinition
    {
        void DefineServices(IServiceCollection serviceCollection);
        void DefineEndpoints(WebApplication app);
    }
}
