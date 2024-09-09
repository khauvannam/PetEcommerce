using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product_API.Databases;
using Product_API.Features.Products;
using Product_API.Interfaces;
using Product_API.Repositories;
using Product_API.Services;
using Shared.Domain.Services;
using Shared.Extensions;

namespace Product_API.Extensions;

public static class Extension
{
    public static void AddPersistence(this IServiceCollection services)
    {
        //localhost:8075
        services
            .AddControllers()
            .AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );

        services.AddHangFireConfig();

        services.AddHttpContextAccessor();
        services.AddBlobService();

        services.AddValidatorsFromAssemblyContaining<GetAllProducts.Validator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped(typeof(DiscountService));
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var conn = ConnString.SqlServer();
        builder.Services.AddDbContext<ProductDbContext>(opt => opt.UseSqlServer(conn));
    }
}
