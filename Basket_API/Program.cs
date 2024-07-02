using Basket_API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();
builder.Services.AddPersistence();

var app = builder.Build();

app.MapControllers();
app.Run();
