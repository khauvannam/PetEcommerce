using Microsoft.Extensions.DependencyInjection;
using Shared.Entities.Services;
using Shared.Extensions.Configurations;
using Shared.Extensions.JwtHandlers;

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
}
