using Basket_API.Database;
using Basket_API.Interfaces;
using Basket_API.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Basket_API.Extensions;

public static class Extension
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        // 172.20.0.2
        var conn =
            "Server=localhost,8071;Initial Catalog=BasketDatabase;User ID=sa;Password=Nam09189921;TrustServerCertificate=True;Encrypt=false";
        builder.Services.AddDbContext<BasketDbContext>(opt => opt.UseSqlServer(conn));
    }

    public static void AddPersistence(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IBasketItemRepository, BasketItemRepository>();
    }
}
