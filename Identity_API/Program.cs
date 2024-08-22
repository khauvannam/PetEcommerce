using Identity.API.Extensions;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();
builder.Services.AddControllers();

builder.Services.AddPersistence();
builder.Services.AddBearerConfig();

var app = builder.Build();

app.MapControllers();
app.Run();
