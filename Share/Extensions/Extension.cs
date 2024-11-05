using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Share.Domain.Services;
using Share.Extensions.Configurations;
using Share.Extensions.JwtHandlers;
using Share.Services;

namespace Share.Extensions;

public static class Extension
{
    public static void AddBearerConfig(this IServiceCollection services)
    {
        const string jwtSecret = Key.JwtSecret;
        services
            .AddAuthentication(opt => opt.AuthOptionsConfig())
            .AddJwtBearer(opt => opt.BearerOptionsConfig(jwtSecret));

        services.AddAuthorization(opt => opt.AuthorizationConfig());
        services.AddHttpContextAccessor();
    }

    public static void AddBlobService(this IServiceCollection services)
    {
        services.AddScoped<BlobService>();
    }

    public static void AddUserClaimService(this IServiceCollection services)
    {
        services.AddScoped<UserClaimService>();
    }

    public static void AddMailerService(this IServiceCollection services)
    {
        services.AddSingleton(typeof(EmailService));
    }

    public static void AddCorsAllowAll(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
            );
        });
    }

    public static void AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.AddSwaggerAuth();

            // solve conflict records
            opt.CustomSchemaIds(x => x.FullName);
        });
    }

    public static void UseSwaggerConfig(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return;

        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(opt => opt.AddSwaggerUiConfig());
    }
}
