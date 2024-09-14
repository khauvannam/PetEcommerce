using Product_API.Extensions;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();

var service = builder.Services;

service.AddPersistence();
service.AddSwaggerConfig();
service.AddCorsAllowAll();

var app = builder.Build();

var provider = app.Services;

provider.AddDataSeeder();

app.UseCors("AllowAllOrigins");
app.MapControllers();
app.UseSwaggerConfig();
app.Run();
