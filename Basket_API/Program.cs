using Basket_API.Extensions;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();
builder
    .Services.AddControllers()
    .AddNewtonsoftJson(opt =>
        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    );
builder.Services.AddPersistence();

var app = builder.Build();

app.MapControllers();
app.Run();
