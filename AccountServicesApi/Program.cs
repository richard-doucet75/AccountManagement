using AccountServices.Infrastructure.Gateways;
using AccountServicesApi.EndpointDefinitions;
using AccountServicesApi.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointDefinitions(typeof(CreateAccountEndpointDefinition));
builder.Services.AddDbContext<AccountDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("ACCOUNT_SERVICES_CONNECTION_STRING"))
);
var app = builder.Build();
app.UseHttpsRedirection();
app.UseEndpointDefinitions();

app.Run();





