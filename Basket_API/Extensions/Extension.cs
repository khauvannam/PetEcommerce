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
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST")!;
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")!;
        var conn =
            $"Server={dbHost};Initial Catalog=BasketDatabase;User ID=sa;Password={dbPassword};TrustServerCertificate=True;";
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
