using Microsoft.EntityFrameworkCore;
using AccountServicesApi.ValueTypeConverters;
using AccountServices.Infrastructure.Gateways;
using AccountServicesApi.EndpointDefinitions;
using AccountServicesApi.Utilities;
using Microsoft.Extensions.DependencyInjection;
using AccountServices.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.UseInfrastructure();
services.AddEndpointDefinitions(typeof(CreateAccountEndpointDefinition));
services.AddTypeConverters();
services.AddDbContext<AccountDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("ACCOUNT_SERVICES_CONNECTION_STRING"))
);

services.AddUserContext();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseEndpointDefinitions();
app.Run();





