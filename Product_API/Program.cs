using Carter;
using Product_API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();

var service = builder.Services;
service.AddPersistence();

var app = builder.Build();
app.UseCors("AllowAllOrigins");
app.MapCarter();
app.Run();
