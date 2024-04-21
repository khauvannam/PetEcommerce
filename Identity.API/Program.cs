using Identity.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();

builder.Services.AddControllers();
builder.Services.AddPersistence();
var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
