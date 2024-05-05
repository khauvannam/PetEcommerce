using FluentValidation;
using Identity.API.Databases;
using Identity.API.Entities;
using Identity.API.Interfaces;
using Identity.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions.JwtHandlers;

namespace Identity.API.Extensions;

public static class Extension
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST")!;
        var conn =
            $"Data Source=sql.server:1433;Initial Catalog=UserDatabase;User ID=sa;Password=Nam09189921;TrustServerCertificate=True;";
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
        services.AddScoped<IUserValidateRepository, UserValidateRepository>();
        services.AddTransient<JwtHandler>();
    }
}
