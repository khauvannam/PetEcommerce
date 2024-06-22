using FluentValidation;
using Identity.API.Databases;
using Identity.API.Domain.Users;
using Identity.API.Interfaces;
using Identity.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions.JwtHandlers;
using Shared.Services;

namespace Identity.API.Extensions;

public static class Extension
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST")!;
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")!;
        var conn =
            $"Server={dbHost};Initial Catalog=UserDatabase;User ID=sa;Password={dbPassword};TrustServerCertificate=True;";
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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserServiceRepository, UserServiceRepository>();
        services.AddTransient<JwtHandler>();
    }
}
