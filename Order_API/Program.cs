using Order.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddDatabase();

services.AddPersistence();
services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
