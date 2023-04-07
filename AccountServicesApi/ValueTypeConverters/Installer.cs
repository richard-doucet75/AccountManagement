using AccountServices.UseCases.ValueTypes;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AccountServicesApi.ValueTypeConverters
{
    public static class Installer
    {
        public static void AddTypeConverters(this IServiceCollection serviceCollection)
            => serviceCollection.Configure<JsonOptions>(options =>
               {
                   options.SerializerOptions.Converters.Add(new EmailAddressTypeConverter());
                   options.SerializerOptions.Converters.Add(new PasswordTypeConverter());
               });

        public static void ConfigureSwaggerGen(this SwaggerGenOptions options)
        {
            options.MapType<EmailAddress>(
                    () => new OpenApiSchema
                    {
                        Type = "string",
                        Example = new OpenApiString("local-part@domain.com")
                    });
            options.MapType<Password>(
                () => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("Pa$$word1")
                });

            options.CustomSchemaIds((type) => type.Name.TrimEnd("Model".ToArray()));
        }
    }
}
