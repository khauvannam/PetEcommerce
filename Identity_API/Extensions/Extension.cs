﻿using FluentValidation;
using Identity.API.Databases;
using Identity.API.Domains.Users;
using Identity.API.Interfaces;
using Identity.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;
using Shared.Extensions.JwtHandlers;

namespace Identity.API.Extensions;

public static class Extension
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var conn =
            "Server=localhost,1433;Initial Catalog=UsrDatabase;User ID=sa;Password=Nam09189921;TrustServerCertificate=True;Encrypt=False";
        builder.Services.AddDbContext<UserDbContext>(opt => opt.UseSqlServer(conn));
    }

    public static void AddPersistence(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        services
            .AddIdentity<User, IdentityRole>(opt => opt.SignIn.RequireConfirmedAccount = false)
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<UserDbContext>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        // Add outside services
        services.AddBlobService();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserServiceRepository, UserServiceRepository>();
        services.AddTransient<JwtHandler>();
    }
}
