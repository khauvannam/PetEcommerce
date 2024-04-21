using Ocelot.DependencyInjection;

namespace Gateway.API.Extensions;

public static class Extension
{
    public static void OcelotConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        builder.Services.AddOcelot(builder.Configuration);
    }

    public static void AddPersistence(this IServiceCollection services) { }
}
