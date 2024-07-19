using Microsoft.EntityFrameworkCore;
using Order.API.Databases;
using Order.API.Interfaces;
using Order.API.Repositories;
using IShippingMethodRepository = Order.API.Interfaces.IShippingMethodRepository;

namespace Order.API.Extensions;

public static class Extension
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        // 172.20.0.2
        var conn =
            "Server=localhost,1433;Initial Catalog=OrderDatabase;User ID=sa;Password=Nam09189921;TrustServerCertificate=True";
        builder.Services.AddDbContext<OrderDbContext>(opt => opt.UseSqlServer(conn));
    }

    public static void AddPersistence(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IShippingMethodRepository, ShippingMethodRepository>();
    }
}
