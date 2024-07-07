using Carter;
using Product_API.Databases;
using Product_API.Interfaces;
using Product_API.Repositories;

namespace Product_API.Extensions;

public static class Extension
{
    public static void AddPersistence(this IServiceCollection services)
    {
        var catalog = new DependencyContextAssemblyCatalog();
        var types = catalog.GetAssemblies().SelectMany(x => x.GetTypes());

        var modules = types
            .Where(t =>
                !t.IsAbstract
                && typeof(ICarterModule).IsAssignableFrom(t)
                && (t.IsPublic || t.IsNestedPublic)
            )
            .ToList();

        services.AddCarter(configurator: c =>
        {
            c.WithModules(modules.ToArray());
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder
            .Services.Configure<ProductDatabaseSetting>(
                builder.Configuration.GetSection(nameof(ProductDatabaseSetting))
            )
            .AddOptionsWithValidateOnStart<ProductDatabaseSetting>();

        builder
            .Services.Configure<CategoryDatabaseSetting>(
                builder.Configuration.GetSection(nameof(CategoryDatabaseSetting))
            )
            .AddOptionsWithValidateOnStart<CategoryDatabaseSetting>();
    }
}
