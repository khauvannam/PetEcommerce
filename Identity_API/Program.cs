using Identity.API.Extensions;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabase();
builder.Services.AddControllers();

builder.Services.AddPersistence();
builder.Services.AddBearerConfig();
builder.Services.AddCorsAllowAll();

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.MapControllers();
app.Run();
