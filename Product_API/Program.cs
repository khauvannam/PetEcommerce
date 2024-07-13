using Carter;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Product_API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();

var service = builder.Services;
service.AddPersistence();

var app = builder.Build();
app.MapCarter();
app.Run();
