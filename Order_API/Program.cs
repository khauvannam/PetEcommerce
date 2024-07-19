using Order.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var services = builder.Services;

builder.AddDatabase();
services.AddPersistence();
services.AddControllers();

app.MapControllers();
app.Run();
