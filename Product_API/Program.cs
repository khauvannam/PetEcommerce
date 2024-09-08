using Carter;
using Hangfire;
using Product_API.Extensions;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();

var service = builder.Services;

service.AddPersistence();
service.AddSwaggerConfig();
service.AddCorsAllowAll();

var app = builder.Build();

app.UseHangfireDashboard();
app.UseCors("AllowAllOrigins");
app.MapCarter();
app.UseSwaggerConfig();
app.Run();
