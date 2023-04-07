using Microsoft.EntityFrameworkCore;

using AccountServicesApi.ValueTypeConverters;
using AccountServices.Infrastructure.Gateways;
using AccountServicesApi.EndpointDefinitions;
using AccountServicesApi.Utilities;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddEndpointDefinitions(typeof(CreateAccountEndpointDefinition));
services.AddTypeConverters();
services.AddDbContext<AccountDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("ACCOUNT_SERVICES_CONNECTION_STRING"))
);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseEndpointDefinitions();
app.Run();





