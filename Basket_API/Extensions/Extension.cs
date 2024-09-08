using Basket_API.Database;
using Basket_API.Interfaces;
using Basket_API.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Domain.Services;
using Shared.Extensions;

namespace Basket_API.Extensions;

public static class Extension
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        // 172.20.0.2
        var conn = ConnString.SqlServer();
        builder.Services.AddDbContext<BasketDbContext>(opt => opt.UseSqlServer(conn));
    }

    public static void AddPersistence(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        services.AddValidatorsFromAssembly(assembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddScoped<BasketRepository>();
        services.AddScoped<IBasketRepository>(provider =>
        {
            var repository = provider.GetRequiredService<BasketRepository>();
            var cache = provider.GetRequiredService<IDistributedCache>();
            return new CachedBasketRepository(cache, repository);
        });
        services.AddScoped<IBasketItemRepository, BasketItemRepository>();

        services.AddSwaggerConfig();
    }
}
