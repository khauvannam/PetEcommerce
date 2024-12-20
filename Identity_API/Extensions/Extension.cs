﻿using FluentValidation;
using Identity.API.Databases;
using Identity.API.Domains.Users;
using Identity.API.Interfaces;
using Identity.API.Repositories;
using Identity.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Share.Domain.Services;
using Share.Extensions;
using Share.Extensions.JwtHandlers;

namespace Identity.API.Extensions;

public static class Extension
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var conn = ConnString.SqlServer(database: "UserDatabase");
        builder.Services.AddDbContext<UserDbContext>(opt => opt.UseSqlServer(conn));
    }

    public static void AddPersistence(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        services
            .AddIdentity<User, IdentityRole<int>>(opt => opt.SignIn.RequireConfirmedAccount = false)
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<UserDbContext>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        // Add external services

        services.AddBlobService();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserServiceRepository, UserServiceRepository>();
        services.AddScoped<UserEmailService>();
    }
}
