using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Services;
using Shared.Extensions.Configurations;
using Shared.Extensions.JwtHandlers;
using Shared.Services;

namespace Shared.Extensions;

public static class Extension
{
    public static void AddBearerConfig(this IServiceCollection services)
    {
        const string jwtSecret = Key.JwtSecret;
        services
            .AddAuthentication(opt => opt.AuthOptionsConfig())
            .AddJwtBearer(opt => opt.BearerOptionsConfig(jwtSecret));

        services.AddAuthorization(opt => opt.AuthorizationConfig());
    }

    public static void AddBlobService(this IServiceCollection services)
    {
        services.AddSingleton<BlobService>();
        services.AddSingleton(_ => new BlobServiceClient(Key.BlobConnectionString));
    }

    public static void AddEmailService(this IServiceCollection services)
    {
        services.AddSingleton(typeof(EmailService));
    }
}
