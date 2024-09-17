using Order.API.Extensions;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddDatabase();

services.AddPersistence();
services.AddControllers();
services.AddCorsAllowAll();

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.MapControllers();
app.Run();
