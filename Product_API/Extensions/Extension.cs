﻿using Coravel;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product_API.Databases;
using Product_API.Features.Products;
using Product_API.Interfaces;
using Product_API.Repositories;
using Product_API.Services;
using Share.Domain.Services;
using Share.Extensions;

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

        services.AddHttpContextAccessor();
        services.AddBlobService();

        services.AddValidatorsFromAssemblyContaining<CreateProduct.Validator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // Coravel
        services.AddScheduler();
        services.AddQueue();

        // Repository
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddTransient<ProductService>();

        // Service
        services.AddTransient(typeof(DiscountService));
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var conn = ConnString.SqlServer();
        builder.Services.AddDbContext<ProductDbContext>(opt => opt.UseSqlServer(conn));
    }

    public static void AddDataSeeder(this IServiceProvider serviceProvider)
    {
        DataSeeder.SeedCategory(serviceProvider);
        DataSeeder.SeedProduct(serviceProvider);
    }
}
