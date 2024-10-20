using Client_App.Components.Home;
using Client_App.Interfaces;
using Client_App.Services;
using Client_App.Services.APIs;
using Client_App.Services.Share;

namespace Client_App.Extensions;

public static class Extension
{
    public static void AddHttpClientAddress(this IServiceCollection services)
    {
        services.AddHttpClient(
            nameof(ProductService),
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:5081");
            }
        );

        services.AddHttpClient(
            nameof(BasketService),
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:8080/");
            }
        );

        services.AddHttpClient(
            nameof(IdentityService),
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:5175/");
            }
        );
    }

    public static void AddFetchApiService(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddSingleton<ErrorService>();
        services.AddSingleton<TitleService>();
    }
}
