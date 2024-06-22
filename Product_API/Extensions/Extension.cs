using Product_API.Databases;
using Product_API.Interfaces;
using Product_API.Repositories;

namespace Product_API.Extensions;

public static class Extension
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services
            .Configure<ProductDatabaseSetting>(
                builder.Configuration.GetSection(nameof(ProductDatabaseSetting))
            )
            .AddOptionsWithValidateOnStart<ProductDatabaseSetting>();
    }
}
