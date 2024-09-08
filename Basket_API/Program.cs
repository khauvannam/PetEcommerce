using Basket_API.Extensions;
using Newtonsoft.Json;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();
builder
    .Services.AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
builder.Services.AddPersistence();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    var conn = builder.Configuration.GetConnectionString("RedisUrl");
    if (string.IsNullOrEmpty(conn))
        throw new Exception("The redis connection string is missing.");

    opt.Configuration = conn;
});

var app = builder.Build();

app.MapControllers();
app.UseSwaggerConfig();
app.Run();
