using Basket_API.Extensions;
using Newtonsoft.Json;

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
        throw new Exception("redis conn can't be null");

    opt.Configuration = conn;
});

var app = builder.Build();

app.MapControllers();
app.Run();
