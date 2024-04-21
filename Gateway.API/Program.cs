using Gateway.API.Extensions;
using Ocelot.Middleware;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.OcelotConfiguration();

var services = builder.Services;
services.AddBearerConfig();
var app = builder.Build();

await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();
app.Run();
