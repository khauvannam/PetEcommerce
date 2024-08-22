using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product_API.Databases;
using Product_API.Features.Products;
using Product_API.Interfaces;
using Product_API.Repositories;
using Shared.Domain.Services;
using Shared.Extensions;

namespace Product_API.Extensions;

public static class Extension
{
    public static void AddPersistence(this IServiceCollection services)
    {
        //localhost:8075
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
        services
            .AddControllers()
            .AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
        services.AddScopeService();

        services.AddValidatorsFromAssemblyContaining<GetAllProducts.Validator>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var conn = ConnString.SqlServer("ProductDatabase");
        builder.Services.AddDbContext<ProductDbContext>(opt => opt.UseSqlServer(conn));
    }
}
